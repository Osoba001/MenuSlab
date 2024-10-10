using Main.Application.Requests.OrderRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.OrderHandlers
{
    internal class RemoveItemFromOrderHandler(MenuslabDbContext dbContext) : IRequestHandler<RemoveItemFromOrderRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;
        public async Task<ActionResponse> HandleAsync(RemoveItemFromOrderRequest request, CancellationToken cancellationToken = default)
        {
            var order = await _dbContext.Orders.FindAsync(request.OrderId);
            if (order == null) return NotFoundResult();

            if (order.OrderType != Application.Enums.OrderType.SelfOnPremise) return BadRequestResult("Online order can't be alter");

            if (order.Status == Application.Enums.OrderStatus.Completed) return BadRequestResult("Complete order can't be alter");

            var orderItems = await _dbContext.OrderedItems.Where(x =>x.OrderId==request.OrderId && request.OrderedItems.Keys.Contains(x.ItemId)).ToListAsync();
            _dbContext.OrderedItems.RemoveRange(orderItems);
            return await _dbContext.CompleteAsync();

        }
    }
}
