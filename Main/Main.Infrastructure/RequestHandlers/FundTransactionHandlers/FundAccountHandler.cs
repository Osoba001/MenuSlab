using Main.Application.Requests.FundTransactionRequests;
using Main.Infrastructure.Database;
using Main.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Share.Payments;

namespace Main.Infrastructure.RequestHandlers.FundTransactionHandlers
{
    internal class FundAccountHandler(MenuslabDbContext dbContext, IPaymentGatewayFactory paymentGatewayFactory) : IRequestHandler<FundAccountRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;
        private readonly IPaymentGatewayFactory _paymentGatewayFactory = paymentGatewayFactory;

        public async Task<ActionResponse> HandleAsync(FundAccountRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _dbContext.Users.FindAsync(request.UserIdentifier);
            if (user == null) return NotFoundResult();

            var country= await _dbContext.Countries.Where(x=>x.Name==user.CountryId)
                .Select(x=> new
                {
                    x.Name,
                    x.Currency,
                    ImplementationName= x.PaymentGateway!=null?x.PaymentGateway.ImplementationName:null
                }).FirstOrDefaultAsync();
            if (country == null) return NotFoundResult("Country not found");

            if(country.ImplementationName==null) return BadRequestResult("No payment verification found.");

            if(await _dbContext.Funds.Where(x=>x.TransactionId==request.TransactionId).AnyAsync()) 
                return BadRequestResult("Transaction already added.");// log or email this

            var paymentGateway = _paymentGatewayFactory.GetPaymentGateway(country.ImplementationName);

            if (!await paymentGateway.VerifyReceivedFund(request.TransactionId, request.Amount))
                return BadRequestResult("Your transaction was not successful.");


            using var trans= _dbContext.Database.BeginTransaction();

            try
            {
                user.Balance+= request.Amount;
                _dbContext.Entry(user).Property(x=>x.Balance).IsModified=true;
                _dbContext.Funds.Add(new Fund
                {
                    CreatedDate = DateTime.UtcNow,
                    Amount= request.Amount,
                    FundTransactionType=Application.Enums.FundTransactionType.FundAccount,
                    TransactionId=request.TransactionId,
                    ReceiverId=request.UserIdentifier,
                });
                await _dbContext.SaveChangesAsync();
                await trans.CommitAsync();
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return ServerExceptionError(ex);
            }
            return SuccessResult();
        }
    }
}
