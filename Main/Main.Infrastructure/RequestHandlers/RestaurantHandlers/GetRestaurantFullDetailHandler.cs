using Main.Application.Requests.RestaurantRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.RestaurantHandlers
{
    internal class GetRestaurantFullDetailHandler(MenuslabDbContext dbContext) : IRequestHandler<GetRestaurantFullDetailRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(GetRestaurantFullDetailRequest request, CancellationToken cancellationToken = default)
        {
            var restaurant = await _dbContext.Restaurants.Where(x => x.Id == request.Id)
                .Select(x => new GetRestaurantFullDetailResponse
                {
                    Id = request.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Number = x.Number,
                    PhoneNo = x.PhoneNo,
                    CloseTime = x.CloseTime,
                    OpenTime = x.OpenTime,
                    DeliveryCondiction = x.DeliveryCondiction,
                    HomeDelivery = x.HomeDelivery,
                    Coordinates = new Application.CustomTypes.Coordinates { Latitude = x.Latitude, Longitude = x.Longitude },
                    Country = x.CountryId,
                    Currency = x.Country.Currency,
                    PayWithApp = x.PayWithApp,
                    OnPremise = x.OnPremise,
                    RestaurantRadius = x.RestaurantRadius,
                    StreetAddress = x.StreetAddress,
                    OpenCode = x.OpenCode,
                    BankAccount = x.BankDetails,
                    Charges = x.Charges,
                    IsActive = x.IsActive,
                    IsLocked= x.IsLocked,
                    LockedMessage=x.LockedMessage,
                    IsCreator=x.UserId==request.UserIdentifier,
                    NetBalance = x.NetBalance,
                    Staff = x.Staff.Select(s => new Application.Requests.RestaurantStaffRequests.RestaurantStaffResponse
                    {
                        EndOfDuty = s.EndOfDuty,
                        StartOfDuty = s.StartOfDuty,
                        Name = s.User.Name,
                        Role = s.Role,
                        UserId = s.User.Id,
                        Id = s.Id,
                    })
                }).FirstOrDefaultAsync();
            if (restaurant == null) return NotFoundResult();
            return new ActionResponse { PayLoad = restaurant };
        }
    }
}
