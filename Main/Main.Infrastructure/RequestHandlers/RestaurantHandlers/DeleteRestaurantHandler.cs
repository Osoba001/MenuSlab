using Main.Application.Requests.RestaurantRequests;
using Main.Infrastructure.Database;

namespace Main.Infrastructure.RequestHandlers.RestaurantHandlers
{
    internal class DeleteRestaurantHandler(MenuslabDbContext dbContext) : IRequestHandler<DeleteRestaurantRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(DeleteRestaurantRequest request, CancellationToken cancellationToken = default)
        {
            var restaurant = await _dbContext.Restaurants.FindAsync(request.Id);
            if (restaurant == null) return NotFoundResult();

            var owner = await _dbContext.Users.FindAsync(request.UserIdentifier);
            if (owner == null) return Forbiden();

            if (request.UserDevice.ToString() != owner.UserDevice)
            {
                //send mail
                return Forbiden($"Deletion of your {restaurant.Name} Restaurant attempt detected from an unrecognized device. Your most recent login was from {owner.UserDevice}. Please log in again to authorize the use of your current device ({request.UserDevice.Model}).");
            }

            restaurant.IsDeleted = true;
            restaurant.DeletedDate= DateTime.UtcNow;
            _dbContext.Entry(restaurant).Property(x=>x.IsDeleted).IsModified=true;
            _dbContext.Entry(restaurant).Property(x=>x.DeletedDate).IsModified=true;
            return await _dbContext.CompleteAsync();

        }
    }
}
