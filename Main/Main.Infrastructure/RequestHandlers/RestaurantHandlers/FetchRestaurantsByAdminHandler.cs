using Main.Application.Requests.RestaurantRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.RestaurantHandlers
{
    internal class FetchRestaurantsByAdminHandler(MenuslabDbContext dbContext) : IRequestHandler<FetchRestaurantsByAdminRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(FetchRestaurantsByAdminRequest request, CancellationToken cancellationToken = default)
        {
            if (request.Page <= 0 || request.PageSize <= 0)
                return BadRequestResult("Invalid Page or PageSize values.");

            int offset = (request.Page - 1) * request.PageSize;

            return new ActionResponse
            {
                PayLoad = await _dbContext.Restaurants.Where(x => x.CountryId == request.Country)
                .OrderBy(x => x.Number)
                .Skip(offset)
                .Take(request.PageSize)
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
