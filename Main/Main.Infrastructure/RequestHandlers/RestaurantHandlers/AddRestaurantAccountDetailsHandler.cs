using Main.Application.Requests.RestaurantRequests;
using Main.Infrastructure.Database;

namespace Main.Infrastructure.RequestHandlers.RestaurantHandlers
{
    internal class AddRestaurantAccountDetailsHandler(MenuslabDbContext dbContext) : IRequestHandler<AddRestaurantAccountDetailsRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(AddRestaurantAccountDetailsRequest request, CancellationToken cancellationToken = default)
        {
            var restaurant = await _dbContext.Restaurants.FindAsync( request.Id);
            if (restaurant == null) return NotFoundResult();

            var owner= await _dbContext.Users.FindAsync( request.UserIdentifier);
            if (owner == null) return Forbiden();

            if (request.UserDevice.ToString() != owner.UserDevice)
            {
                
                return Forbiden($"Bank datails update attempt detected from an unrecognized device. Your most recent login was from {owner.UserDevice}. Please log in again to authorize the use of your current device ({request.UserDevice.Model}).");
            }

            if (restaurant.BankDetails == null)
            {
                restaurant.BanckDetailVerified = true;
            }else
            {
                if (restaurant.BankDetails.Bank.Equals(request.BankAccountDetails.Bank, StringComparison.CurrentCultureIgnoreCase)
                   && restaurant.BankDetails.AccountNo == request.BankAccountDetails.AccountNo
                   && restaurant.BankDetails.AccountName.Equals(request.BankAccountDetails.AccountName, StringComparison.CurrentCultureIgnoreCase))
                {
                    return SuccessResult();
                }
                restaurant.BanckDetailVerified = false;
            }

            restaurant.WithrewWithThisAccount = 0;
            restaurant.PayWithApp=true;

            restaurant.BankDetails= request.BankAccountDetails;
            
            _dbContext.Restaurants.Update(restaurant);
            //send mail if successful
            return await _dbContext.CompleteAsync();
        }
    }
}
