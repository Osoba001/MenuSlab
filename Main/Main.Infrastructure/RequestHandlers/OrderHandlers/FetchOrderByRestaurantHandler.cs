using Main.Application.Requests.OrderRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.OrderHandlers
{
    internal class FetchOrderByRestaurantHandler(MenuslabDbContext dbContext) : IRequestHandler<FetchOrderByRestaurantRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(FetchOrderByRestaurantRequest request, CancellationToken cancellationToken = default)
        {
            if (request.Page <= 0 || request.PageSize <= 0)
                return BadRequestResult("Invalid Page or PageSize values.");

            int offset = (request.Page - 1) * request.PageSize;

            return new ActionResponse
            {
                PayLoad = await _dbContext.Orders.Where(x => x.ReceiverId == request.RestaurantId)
                .OrderByDescending(x => x.CreatedDate)
                .Skip(offset).Take(request.PageSize)
                .Select(x => new FetchOrderByRestaurantResponse
                {
                    RestaurantAmount = x.RestaurantAmount,
                    OrderCharges = x.OrderCharges,
                    TableNumber = x.TableNumber,
                    OrderType = x.OrderType.ToString(),
                    CreatedDate = x.CreatedDate,
                    RestaurantId = x.RestaurantId,
                    RestaurantName = x.Restaurant.Name,
                    Status = x.Status,
                    UserId = x.UserId,
                    UserName = x.User.Name,
                    DeliveryCoordinate = x.DeliveryCoordinates,
                    Id = x.Id,
                    OrderNumber = x.OrderNumber,
                    OrderPaid = x.OrderPaid,
                    DeleveryFee=x.DeliveryFee,
                })
                .ToListAsync()

            };
        }
    }
}
