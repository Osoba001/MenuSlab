using Main.Application.Enums;
using Main.Application.Requests.OrderRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.OrderHandlers
{
    internal class FetchOrderByUserHandler(MenuslabDbContext dbContext) : IRequestHandler<FetchOrderByUserRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(FetchOrderByUserRequest request, CancellationToken cancellationToken = default)
        {
            return new ActionResponse
            {
                PayLoad = await _dbContext.Orders.Where(x => (x.UserId == request.UserIdentifier && x.Delete != DeleteStatus.Sender) || (x.ReceiverId == request.UserIdentifier && x.Delete != DeleteStatus.Sender))
                .OrderByDescending(x => x.CreatedDate)
                .Take(20)
                .Select(x => new FetchOrderResponse
                {
                    Amount = x.RestaurantAmount,
                    Charges = x.OrderCharges,
                    TableNumber = x.TableNumber,
                    OrderType = x.OrderType.ToString(),
                    CreatedDate = x.CreatedDate,
                    RestaurantId = x.RestaurantId,
                    RestaurantName = x.Restaurant.Name,
                    ReceiverId = x.ReceiverId,
                    Receiver = x.Receiver != null ? x.Receiver.Name : null,
                    Status = x.Status,
                    UserId = x.UserId,
                    UserName = x.User.Name,
                    Id = x.Id,
                    OrderNumber = x.OrderNumber,
                })
                .ToListAsync()

            };
        }
    }
}
