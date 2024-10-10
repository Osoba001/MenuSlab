using Main.Application.Requests.RestaurantRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.RestaurantHandlers
{
    internal class UpdateLockRestaurantStatusHandler(MenuslabDbContext dbContext) : IRequestHandler<UpdateLockRestaurantStatusRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(UpdateLockRestaurantStatusRequest request, CancellationToken cancellationToken = default)
        {
            var isAmin= await _dbContext.SystemStaff.Where(x=>x.UserId==request.UserIdentifier && x.IsActive).FirstOrDefaultAsync();
            if (isAmin == null) return Forbiden();
            var resturant= await _dbContext.Restaurants.Where(x=>x.Id==request.Id).FirstOrDefaultAsync();
            if(resturant == null)
                return NotFoundResult();
            resturant.IsLocked = resturant.IsLocked;
            resturant.LockedMessage= resturant.LockedMessage;

            _dbContext.Entry(resturant).Property(x=>x.IsLocked).IsModified =true;
            _dbContext.Entry(resturant).Property(x=>x.LockedMessage).IsModified =true;
            return await _dbContext.CompleteAsync();
        }
    }
}
