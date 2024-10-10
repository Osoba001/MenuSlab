using Main.Application.Requests.RestaurantRequests;
using Main.Infrastructure.Database;

namespace Main.Infrastructure.RequestHandlers.RestaurantHandlers
{
    internal class UpdateRestaurantHandler(MenuslabDbContext dbContext) : IRequestHandler<UpdateRestaurantRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(UpdateRestaurantRequest request, CancellationToken cancellationToken = default)
        {
            var restaurant = await _dbContext.Restaurants.FindAsync(request.Id);
            if (restaurant == null) return NotFoundResult();

            var owner = await _dbContext.Users.FindAsync(request.UserIdentifier);
            if (owner == null) return Forbiden();

            if (request.UserDevice.ToString() != owner.UserDevice)
            {
                //send mail
                return Forbiden($"Update of your {restaurant.Name} Restaurant attempt detected from an unrecognized device. Your most recent login was from {owner.UserDevice}. Please log in again to authorize the use of your current device ({request.UserDevice.Model}).");
            }

            var change = new { restaurant.Name, restaurant.CloseTime, restaurant.Description, restaurant.HomeDelivery, restaurant.DeliveryCondiction, restaurant.Latitude, restaurant.Longitude,
                restaurant.OnPremise, restaurant.StreetAddress,restaurant.OpenTime, restaurant.PayWithApp, restaurant.PhoneNo, restaurant.IsActive,restaurant.RestaurantRadius };

            restaurant.Name=request.Name;
            restaurant.CloseTime = request.CloseTime;
            restaurant.Description=request.Description;
            restaurant.HomeDelivery=request.HomeDelivery;
            restaurant.DeliveryCondiction=request.DeliveryCondiction;
            restaurant.Latitude=request.Coordinates.Latitude;
            restaurant.Longitude=request.Coordinates.Longitude;
            restaurant.OnPremise=request.OnPremise;
            restaurant.StreetAddress=request.StreetAddress;
            restaurant.OpenTime=request.OpenTime;
            restaurant.PayWithApp=request.PayWithApp;
            restaurant.PhoneNo=request.PhoneNo;
            restaurant.IsActive=request.IsActive;
            restaurant.RestaurantRadius=request.RestaurantRadius;

            _dbContext.Entry(restaurant).Property(x=>x.Name).IsModified=change.Name!=request.Name;
            _dbContext.Entry(restaurant).Property(x=>x.CloseTime).IsModified=change.CloseTime!=request.CloseTime;
            _dbContext.Entry(restaurant).Property(x=>x.Description).IsModified=change.Description!=request.Description;
            _dbContext.Entry(restaurant).Property(x=>x.HomeDelivery).IsModified=change.HomeDelivery!=request.HomeDelivery;
            _dbContext.Entry(restaurant).Property(x=>x.DeliveryCondiction).IsModified=change.DeliveryCondiction!=request.DeliveryCondiction;
            _dbContext.Entry(restaurant).Property(x=>x.Latitude).IsModified=change.Latitude!=request.Coordinates.Latitude;
            _dbContext.Entry(restaurant).Property(x=>x.Longitude).IsModified=change.Longitude!=request.Coordinates.Longitude;
            _dbContext.Entry(restaurant).Property(x=>x.OnPremise).IsModified=change.OnPremise!=request.OnPremise;
            _dbContext.Entry(restaurant).Property(x=>x.StreetAddress).IsModified=change.StreetAddress!=request.StreetAddress;
            _dbContext.Entry(restaurant).Property(x=>x.OpenTime).IsModified=change.OpenTime!=request.OpenTime;
            _dbContext.Entry(restaurant).Property(x=>x.PayWithApp).IsModified=change.PayWithApp!=request.PayWithApp;
            _dbContext.Entry(restaurant).Property(x=>x.PhoneNo).IsModified=change.PhoneNo!=request.PhoneNo;
            _dbContext.Entry(restaurant).Property(x=>x.IsActive).IsModified=change.IsActive!=request.IsActive;
            _dbContext.Entry(restaurant).Property(x=>x.RestaurantRadius).IsModified=change.RestaurantRadius!=request.RestaurantRadius;

            return await _dbContext.CompleteAsync();
        }
    }
}
