using Main.Application.Requests.RestaurantRequests;
using Main.Infrastructure.Database;
using Main.Infrastructure.Database.Entities;

namespace Main.Infrastructure.RequestHandlers.RestaurantHandlers
{
    internal class CreateRestaurantHandler(MenuslabDbContext dbContext) : IRequestHandler<CreateRestaurantRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(CreateRestaurantRequest request, CancellationToken cancellationToken = default)
        {
            var country= await _dbContext.Countries.FindAsync( request.Country);
            if (country == null) return NotFoundResult();

            var restaurant= new Restaurant 
            {
                Latitude = request.Coordinates.Latitude, 
                Longitude = request.Coordinates.Longitude, 
                CountryId=request.Country, 
                Currency=country.Currency, 
                CloseTime=request.CloseTime,
                OpenTime=request.OpenTime,
                HomeDelivery=request.HomeDelivery,
                UserId=request.UserIdentifier,
                Name=request.Name,
                Description=request.Description,
                OnPremise=request.OnPremise,
                PhoneNo=request.PhoneNo, 
                StreetAddress=request.StreetAddress,
            };
            _dbContext.Restaurants.Add(restaurant);

            _dbContext.RestaurantStaff.Add(new RestaurantStaff
            {
                EndOfDuty=request.CloseTime,
                StartOfDuty=request.OpenTime,
                UserId = request.UserIdentifier,
                RestaurantId=restaurant.Id,
                Role="Manager",
            });

            return await _dbContext.CompleteAsync(new {restaurant.Id});
        }
    }
}
