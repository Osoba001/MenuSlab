using Main.Application.Requests.RestaurantRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.RestaurantHandlers
{
    internal class FetchRestaurantsByUserHandler(MenuslabDbContext dbContext) : IRequestHandler<FetchRestaurantsByUserRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(FetchRestaurantsByUserRequest request, CancellationToken cancellationToken = default)
        {
            return new ActionResponse
            {
                PayLoad = await _dbContext.Restaurants.Where(x => x.Staff.Where(x=>x.UserId==request.UserIdentifier).Any())
                .OrderBy(x => x.Number)
                .Select(x => new FetchRestaurantResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Number = x.Number,
                    Coutry = x.CountryId,
                    PayWithApp = x.PayWithApp,
                    HomeDelivery = x.HomeDelivery,
                    OnPremise = x.OnPremise,
                    StreetAddress = x.StreetAddress,
                    Rates = x.Ratings.Select(x => x.Value)
                }).ToListAsync()
            };
        }
    }
}
