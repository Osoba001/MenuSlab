using Main.Application.Requests.OrderRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.OrderHandlers
{
    internal class GetOrderDetailHandler(MenuslabDbContext dbContext) : IRequestHandler<GetOrderDetailRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(GetOrderDetailRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _dbContext.Orders.Where(x => x.Id == request.Id)
                .Select(x => new GetOrderDetailsResponse
                {
                    Id = x.Id,
                    DeclineMessage = x.DeclineMessage,
                    DeliveryCoordinates = x.DeliveryCoordinates,
                    DeliveryFee = x.DeliveryFee,
                    Status = x.Status,
                    OrderPaid = x.OrderPaid,
                    DeleveryPaid=x.DeliveryPaid,
                    UserId = x.UserId,
                    UserName = x.User.Name,
                    DispatcherId = x.DispatcherId,
                    DispatcherName = x.Dispatcher != null ? x.Dispatcher.User.Name : null,
                    CreatedDate = x.CreatedDate,
                    Currency = x.Restaurant.Country.Currency,
                    RestaurantStaffId = x.RestaurantStaffId,
                    RestaurantStaffName = x.RestaurantStaff != null ? x.RestaurantStaff.User.Name : null,
                    CustomerMessage = x.Completedby,
                    OrderCharges = x.OrderCharges,
                    OrderType = x.OrderType.ToString(),
                    OrderNumber = x.OrderNumber,
                    ReceiverId = x.ReceiverId,
                    ReceiverName = x.Receiver != null ? x.Receiver.Name : null,
                    RestaurantAmount = x.RestaurantAmount,
                    WaitTimeInMinutes = x.WaitTimeInMinutes,
                    TableNumber = x.TableNumber,
                    RestaurantId = x.RestaurantId,
                    RestaurantName = x.Restaurant.Name,
                    OrderedItems = x.OrderedItems.Select(o => new OrderedItemResponse { ItemName = o.Item.Name, Quantity = o.Quantity, UnitPrice = o.UnitPrice }).ToList(),
                }).FirstOrDefaultAsync();
            if (response == null)
                return NotFoundResult();
            return new ActionResponse { PayLoad = response };
        }
    }
    internal class RateOrderHandler(MenuslabDbContext dbContext) : IRequestHandler<RateOrderRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(RateOrderRequest request, CancellationToken cancellationToken = default)
        {
            var order = await _dbContext.Orders.Where(x=>x.Id==request.OrderId)
                .Select(x=> new {x.UserId, x.Status,x.RestaurantId, hasRate=x.RestaurantRatings.Any()})
                .FirstOrDefaultAsync();
            if (order == null) return NotFoundResult("Order not found");

            if (request.UserIdentifier != order.UserId) return Forbiden("You can't rate this order because was not made by you.");

            if (order.Status != Application.Enums.OrderStatus.Completed) return BadRequestResult("Order has not mark completed.");

            if (order.hasRate) return BadRequestResult("This order has been rated already.");

            _dbContext.RestaurantRatings.Add(new Database.Entities.RestaurantRating
            {
                Message = request.Message,
                OrderId = request.OrderId,
                RestaurantId = order.RestaurantId,
                Value = request.Value,
            });
            return await _dbContext.CompleteAsync(cancellationToken);
        }
    }
}
