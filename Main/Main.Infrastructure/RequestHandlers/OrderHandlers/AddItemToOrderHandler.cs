using Main.Application.Requests.OrderRequests;
using Main.Infrastructure.Database;
using Main.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.OrderHandlers
{
    internal class AddItemToOrderHandler(MenuslabDbContext dbContext) : IRequestHandler<AddItemToOrderRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;
        public async Task<ActionResponse> HandleAsync(AddItemToOrderRequest request, CancellationToken cancellationToken = default)
        {
            var order= await _dbContext.Orders.FindAsync(request.OrderId);
            if (order == null) return NotFoundResult();

            if (order.OrderType != Application.Enums.OrderType.SelfOnPremise) return BadRequestResult("Online order can't be alter");

            if (order.Status == Application.Enums.OrderStatus.Completed) return BadRequestResult("Complete order can't be alter");

            var menuItems = await _dbContext.MenuItems.Where(x => request.OrderedItems.Keys.Contains(x.Id)).ToDictionaryAsync(x => x.Id, x => x);
            var orderItems = new List<OrderedItem>();
            foreach (var item in request.OrderedItems)
            {
                if (menuItems.TryGetValue(item.Key, out var menuItem))
                {
                    orderItems.Add(new OrderedItem { ItemId = menuItem.Id, OrderId = request.OrderId, Quantity = item.Value, UnitPrice = menuItem.Price });
                }
            }
            order.AmountChargeBaseOn+= orderItems.Sum(x => x.Quantity * x.UnitPrice);
            _dbContext.Entry(order).Property(x=>x.AmountChargeBaseOn).IsModified=true;
            _dbContext.OrderedItems.AddRange(orderItems);
            return await _dbContext.CompleteAsync();

        }
    }
}
