using Main.Application.Requests.FundTransactionRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.FundTransactionHandlers
{
    internal class TransferFundHandler(MenuslabDbContext dbContext) : IRequestHandler<TransferFundRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(TransferFundRequest request, CancellationToken cancellationToken = default)
        {
            var users = await _dbContext.Users
                .Where(x => x.Id == request.UserIdentifier || x.Id == request.ReceiverId)
                .ToDictionaryAsync(x => x.Id, x => x);
            if (!users.TryGetValue(request.UserIdentifier, out var sender))
                return NotFoundResult("sender not found.");
            
            if (!users.TryGetValue(request.ReceiverId, out var receiver))
                return NotFoundResult("Recipient not found.");

            if (sender.CountryId != receiver.CountryId)
                return Forbiden($"Cross boundary permission denied: Recipient is a different country [{receiver.CountryId}].");

            if (sender.UserDevice != request.UserDevice.ToString())
                return Forbiden($"Transaction attempt detected from an unrecognized device. Your most recent login was from {sender.UserDevice}. Please log in again to authorize the use of your current device ({request.UserDevice.Model}).");

            if (sender.Balance < request.Amound)
                return BadRequestResult("Insufficient fund.");

            using var trans= _dbContext.Database.BeginTransaction();
            try
            {
                sender.Balance -= request.Amound;
                receiver.Balance += request.Amound;
                _dbContext.Entry(sender).Property(x=>x.Balance).IsModified=true;
                _dbContext.Entry(receiver).Property(x=>x.Balance).IsModified=true;

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
