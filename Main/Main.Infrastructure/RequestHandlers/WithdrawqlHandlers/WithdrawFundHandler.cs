using Main.Application.Enums;
using Main.Application.Requests.WithdrawalRequests;
using Main.Infrastructure.Database;
using Main.Infrastructure.Database.Entities;
using Main.Infrastructure.Database.Entities.EntityBase;
using Microsoft.EntityFrameworkCore;
using Share.Payments;

namespace Main.Infrastructure.RequestHandlers.WithdrawqlHandlers
{
    internal class WithdrawFundHandler(MenuslabDbContext dbContext, IPaymentGatewayFactory paymentGatewayFactory) : IRequestHandler<WithdrawFundRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;
        private readonly IPaymentGatewayFactory _paymentGatewayFactory = paymentGatewayFactory;

        public async Task<ActionResponse> HandleAsync(WithdrawFundRequest request, CancellationToken cancellationToken = default)
        {
            if (request.SourceType==WithdrawalSourceType.Dispatcher)
            {
                OrderProviderBase? dispatcher = await _dbContext.Dispatchers.Where(x => x.UserId == request.UserIdentifier).FirstOrDefaultAsync();
                if (dispatcher == null) return NotFoundResult();

                return await HandleWithdrawalAsync(dispatcher, request.SourceType, request.Amount);
            }

            OrderProviderBase? restaurant = await _dbContext.Restaurants.Where(x => x.UserId == request.UserIdentifier).FirstOrDefaultAsync();
            if (restaurant == null) return NotFoundResult();
            return await HandleWithdrawalAsync(restaurant, request.SourceType, request.Amount);
        }


        private async Task<ActionResponse> HandleWithdrawalAsync<T>(T entity, WithdrawalSourceType sourceType, decimal amount)
        where T :  OrderProviderBase
        {
            if (entity == null) return NotFoundResult();

            if (entity.BankDetails == null)
                return BadRequestResult("Bank details has not been set.");

            if (entity.NetBalance < amount)
                return BadRequestResult("Insufficient fund");

            var country = await _dbContext.Countries.Where(x => x.Name == entity.CountryId)
                .Select(x => new
                {
                    x.Currency,
                    x.MaxWithDrawal,
                    x.MinWithDrawal,
                    Implementation = x.PaymentGateway != null ? x.PaymentGateway.ImplementationName : null
                }).FirstOrDefaultAsync();

            if (country == null || country.Implementation == null) return NotFoundResult("Invalid country or Payment gateway");

            if (amount < country.MinWithDrawal)
                return BadRequestResult($"{country.Currency}{amount} is below the minimum withdrawable [{country.Currency}{country.MinWithDrawal}] amount.");
            if (amount > country.MaxWithDrawal)
                return BadRequestResult($"{country.Currency}{amount} is more than the maximum withdrawable [{country.Currency}{country.MaxWithDrawal}] amount.");

            

            var withdraw = new Withdrawal
            {
                Amount = amount,
                NetBalance = entity.NetBalance,
                CountryId = entity.CountryId,
                SourceType = sourceType,
                BankDetails = entity.BankDetails,
                Number = entity.Number
            };

            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                entity.Balance -= amount;
                _dbContext.Entry(entity).Property(x => x.Balance).IsModified = true;

                _dbContext.WithdrewFunds.Add(withdraw);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ServerExceptionError(ex);
            }

            if (entity.WithrewWithThisAccount < 3)
                return new ActionResponse { PayLoad = new { Status = "Pending for approver." } };

            var paymentGateway = _paymentGatewayFactory.GetPaymentGateway(country.Implementation);

            using var payoutTrans = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                withdraw.WithdrawStatus = WithdrawStatus.Withdrew;
                _dbContext.Entry(withdraw).Property(x => x.WithdrawStatus).IsModified = true;
                await _dbContext.SaveChangesAsync();

                var processPayout = await paymentGateway.ProcessWithdrawal(new WithdrawDto
                {
                    AccountName = withdraw.BankDetails.AccountName,
                    AccountNo = withdraw.BankDetails.AccountNo,
                    BankCode = withdraw.BankDetails.Bank,
                    Amount = withdraw.Amount,
                    Currency = country.Currency,
                    Narration = "Menu slap withdrawal.",
                    ReferenceNo = withdraw.Id.ToString("N")
                });
                if (!processPayout.IsSuccess)
                    throw new Exception(processPayout.Message);

                await payoutTrans.CommitAsync();
            }
            catch (Exception ex)
            {
                await payoutTrans.RollbackAsync();
                return ServerExceptionError(ex);
            }

            return new ActionResponse { PayLoad = new { Status = "Withdrawal approved." } };
        }

    }
}
