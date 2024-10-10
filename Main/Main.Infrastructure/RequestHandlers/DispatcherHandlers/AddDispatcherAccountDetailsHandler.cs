using Main.Application.CustomTypes;
using Main.Application.Requests.DispatcherRequests;
using Main.Infrastructure.Database;
using Main.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.DispatcherHandlers
{
    internal class AddDispatcherAccountDetailsHandler(MenuslabDbContext dbContext) : IRequestHandler<AddDispatcherAccountDetailsRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(AddDispatcherAccountDetailsRequest request, CancellationToken cancellationToken = default)
        {
            var dispatcher = await _dbContext.Dispatchers.Where(x => x.UserId == request.UserIdentifier).FirstOrDefaultAsync();
            if (dispatcher == null) return NotFoundResult();

            var owner = await _dbContext.Users.FindAsync(request.UserIdentifier);
            if (owner == null) return Forbiden();

            if (request.UserDevice.ToString() != owner.UserDevice)
            {
                return Forbiden($"Bank datails update attempt detected from an unrecognized device. Your most recent login was from {owner.UserDevice}. Please log in again to authorize the use of your current device ({request.UserDevice.Model}).");
            }

            if (dispatcher.BankDetails==null)
            {
                dispatcher.BanckDetailVerified = true;
            }else
            {
                if(dispatcher.BankDetails.Bank.Equals(request.Bank, StringComparison.CurrentCultureIgnoreCase)
                    && dispatcher.BankDetails.AccountNo==request.AccountNo 
                    && dispatcher.BankDetails.AccountName.Equals(request.AccountName, StringComparison.CurrentCultureIgnoreCase))
                {
                    return SuccessResult();
                }
                dispatcher.BanckDetailVerified = false;
            }
            dispatcher.WithrewWithThisAccount = 0;
            dispatcher.BankDetails=new BankAccountDetails { AccountName=request.AccountName, AccountNo=request.AccountNo, Bank=request.Bank}; 
            _dbContext.Update(dispatcher);
            //send mail if successful
            return await _dbContext.CompleteAsync();
        }
    }
}
