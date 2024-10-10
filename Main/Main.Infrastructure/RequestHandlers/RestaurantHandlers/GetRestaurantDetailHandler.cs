using Main.Application.Requests.RestaurantRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.RestaurantHandlers
{
    internal class GetRestaurantDetailHandler(MenuslabDbContext dbContext) : IRequestHandler<GetRestaurantDetailRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(GetRestaurantDetailRequest request, CancellationToken cancellationToken = default)
        {
            var restaurant = await _dbContext.Restaurants.Where(x => x.Id == request.Id)
                .Select(x => new GetRestaurantDetailResponse
                {
                    Id= request.Id,
                    Name=x.Name,
                    Description = x.Description,
                    Number = x.Number,
                    PhoneNo = x.PhoneNo,
                    CloseTime = x.CloseTime,
                    OpenTime = x.OpenTime,
                    DeliveryCondiction = x.DeliveryCondiction,
                    HomeDelivery= x.HomeDelivery,
                    Coordinates = new Application.CustomTypes.Coordinates { Latitude=x.Latitude, Longitude=x.Longitude},
                    Country= x.CountryId,
                    Currency=x.Country.Currency,
                    PayWithApp=x.PayWithApp,
                    OnPremise=x.OnPremise,
                    RestaurantRadius=x.RestaurantRadius,
                    StreetAddress=x.StreetAddress,
                    BankAccountDetails=x.BankDetails,
                }).FirstOrDefaultAsync();
            if (restaurant == null) return NotFoundResult();
            return new ActionResponse { PayLoad = restaurant };
        }
    }
}
