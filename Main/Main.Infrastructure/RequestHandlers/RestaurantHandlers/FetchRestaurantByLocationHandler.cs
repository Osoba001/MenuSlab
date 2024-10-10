using Main.Application.Requests.RestaurantRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.RestaurantHandlers
{
    internal class FetchRestaurantByLocationHandler(MenuslabDbContext dbContext) : IRequestHandler<FetchRestaurantByLocationRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(FetchRestaurantByLocationRequest request, CancellationToken cancellationToken = default)
        {
            decimal range = 0.75m;
            decimal loLat = request.Latitude - range;
            decimal upLat = request.Latitude + range;
            decimal loLng = request.Longitude - range;
            decimal upLng = request.Longitude + range;

            return new ActionResponse
            {
                PayLoad = await _dbContext.Restaurants
                .Where(x => x.IsActive && !x.IsLocked && x.Latitude > loLat && x.Latitude < upLat && x.Longitude > loLng && x.Longitude < upLng)
                .Select(x => new FetchRestaurantByLocationResponse
                {
                    Id = x.Id,
                    CloseTime = x.CloseTime,
                    OpenCode = x.OpenCode,
                    OpenTime = x.OpenTime,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    Name = x.Name,
                    Number = x.Number,
                    StreetAddress = x.StreetAddress,
                    RestaurantRadius = x.RestaurantRadius,
                    Rates=x.Ratings.Select(x=>x.Value)
                }).ToListAsync()
            };
        }
    }
}
