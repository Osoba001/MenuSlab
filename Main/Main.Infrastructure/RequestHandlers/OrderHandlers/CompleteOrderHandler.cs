using Main.Application.Enums;
using Main.Application.Requests.OrderRequests;
using Main.Infrastructure.Database;

namespace Main.Infrastructure.RequestHandlers.OrderHandlers
{
    internal class CompleteOrderHandler(MenuslabDbContext dbContext) : IRequestHandler<CompleteOrderRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(CompleteOrderRequest request, CancellationToken cancellationToken = default)
        {
            var order = await _dbContext.Orders.FindAsync(request.Id);
            if (order == null) return NotFoundResult();

            var completedBy = "Completed by provider.";
            if (order.UserId != request.UserIdentifier)
                completedBy = "Comleted by Client.";
            if(order.Status==OrderStatus.PendingByDispatcher||order.Status== OrderStatus.PendingByRestaurant)
            {
                order.Status= OrderStatus.Completed;
                order.Completedby= completedBy;
                _dbContext.Entry(order).Property(x=>x.Status).IsModified=true;
                _dbContext.Entry(order).Property(x=>x.Completedby).IsModified=true;
                return await _dbContext.CompleteAsync();
            }
            return BadRequestResult("Not a pending order.");
        }
    }
}
