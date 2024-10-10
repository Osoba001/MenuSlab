using Main.Application.Requests.OrderRequests;
using Main.Infrastructure.Database;
using Main.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.OrderHandlers
{
    internal class CreateOrderHandler(MenuslabDbContext dbContext) : IRequestHandler<CreateOrderRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
        {
            var order = new Order
            {
                UserId = request.UserIdentifier,
                DeliveryCoordinates = request.DeliveryCoordinates,
                RestaurantId = request.RestaurantId,
                OrderType = request.OrderType,
                ReceiverId = request.ReceiverId,
                TableNumber = request.TableNumber,
            };

            


            var menuItems = await _dbContext.MenuItems.Where(x => request.OrderedItems.Keys.Contains(x.Id)).ToDictionaryAsync(x => x.Id, x => x);
            var orderItems = new List<OrderedItem>();
            foreach (var item in request.OrderedItems)
            {
                if (menuItems.TryGetValue(item.Key, out var menuItem))
                {
                    orderItems.Add(new OrderedItem { ItemId = menuItem.Id, OrderId = order.Id, Quantity = item.Value, UnitPrice = menuItem.Price });
                }
            }
            order.AmountChargeBaseOn= orderItems.Sum(x=>x.Quantity*x.UnitPrice);
            _dbContext.Orders.Add(order);
            _dbContext.OrderedItems.AddRange(orderItems);
            return await _dbContext.CompleteAsync(new { order.Id });

        }
    }
}
