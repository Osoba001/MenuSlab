using Main.Application.Enums;
using Main.Application.Requests.WithdrawalRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Share.Payments;

namespace Main.Infrastructure.RequestHandlers.WithdrawqlHandlers
{
    internal class ApproveWithdrawFundHandler(MenuslabDbContext dbContext, IPaymentGatewayFactory paymentGatewayFactory) : IRequestHandler<ApproveWithdrawFundRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;
        private readonly IPaymentGatewayFactory _paymentGatewayFactory = paymentGatewayFactory;

        public async Task<ActionResponse> HandleAsync(ApproveWithdrawFundRequest request, CancellationToken cancellationToken = default)
        {
            var withdrawal = await _dbContext.WithdrewFunds.Where(x => x.Id == request.Id).FirstOrDefaultAsync();
            if (withdrawal == null) return NotFoundResult();

            if (withdrawal.WithdrawStatus != WithdrawStatus.Pending) return BadRequestResult("It's not a pending withdrawal.");
            if (request.Approve)
            {
                if (withdrawal.SourceType == WithdrawalSourceType.Restaurant)
                {
                    var restaurant = await _dbContext.Restaurants.FindAsync(withdrawal.RestaurantId);
                    if (restaurant == null)
                        return ServerExceptionError(new NullReferenceException("Restaurant not found"));
                    restaurant.WithrewWithThisAccount++;
                    _dbContext.Entry(restaurant).Property(x => x.WithrewWithThisAccount).IsModified = true;
                }
                else if (withdrawal.SourceType == WithdrawalSourceType.Dispatcher)
                {
                    var dispatcher = await _dbContext.Dispatchers.FindAsync(withdrawal.DispatcherId);
                    if (dispatcher == null)
                        return ServerExceptionError(new NullReferenceException("Dispatcher not found"));
                    dispatcher.WithrewWithThisAccount++;
                    _dbContext.Entry(dispatcher).Property(x => x.WithrewWithThisAccount).IsModified = true;
                }
            }
            await _dbContext.CompleteAsync();

            var country = await _dbContext.Countries.Where(x => x.Name == withdrawal.CountryId)
                .Select(x => new
                {
                    x.Currency,
                    Implementation = x.PaymentGateway != null ? x.PaymentGateway.ImplementationName : null
                }).FirstOrDefaultAsync();
            if (country == null||country.Implementation==null) return NotFoundResult("Invalid country or Payment gateway");

            using var trans = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                withdrawal.WithdrawStatus = request.Approve ? WithdrawStatus.Approved : WithdrawStatus.Decline;
                withdrawal.SystemAdminId = request.AdminId;
                withdrawal.Message = request.Message;

                _dbContext.WithdrewFunds.Entry(withdrawal).Property(x => x.WithdrawStatus).IsModified = true;
                _dbContext.WithdrewFunds.Entry(withdrawal).Property(x => x.Message).IsModified = true;
                _dbContext.WithdrewFunds.Entry(withdrawal).Property(x => x.SystemAdminId).IsModified = true;

                await _dbContext.SaveChangesAsync(cancellationToken);

                var paymentGateway = _paymentGatewayFactory.GetPaymentGateway(country.Implementation);
                var withdResp = await paymentGateway.ProcessWithdrawal(new WithdrawDto 
                { 
                    AccountName = withdrawal.BankDetails.AccountName, 
                    AccountNo = withdrawal.BankDetails.AccountNo, 
                    BankCode = withdrawal.BankDetails.Bank, 
                    Amount = withdrawal.Amount,
                    Currency=country.Currency,
                    Narration="Menu slap withdrawal.",
                    ReferenceNo=withdrawal.Id.ToString("N")
                });

                if (!withdResp.IsSuccess)
                    throw new Exception(withdResp.Message);
                await trans.CommitAsync(cancellationToken);
                return SuccessResult();
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync(cancellationToken);
                return ServerExceptionError(ex);
            }
        }
    }
}
