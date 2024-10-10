using Main.Application.Requests.OrderRequests;
using Main.Infrastructure.Database;

namespace Main.Infrastructure.RequestHandlers.OrderHandlers
{
    internal class AddDispatcherToOrderHandler(MenuslabDbContext dbContext) : IRequestHandler<AddDispatcherToOrderRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(AddDispatcherToOrderRequest request, CancellationToken cancellationToken = default)
        {
            var order = await _dbContext.Orders.FindAsync(request.Id);

            if (order == null) return NotFoundResult("Order not found");

            var dispatcher = await _dbContext.Dispatchers.FindAsync(order.DispatcherId);
            if (dispatcher == null) return NotFoundResult("Dispatcher not found.");

            using var trans= _dbContext.Database.BeginTransaction();
            try
            {
                dispatcher.Charges += (request.Amount * 0.15m);
                _dbContext.Entry(dispatcher).Property(x=>x.Charges).IsModified=true;

                order.DispatcherId = request.DispatcherId;
                order.DeliveryFee = request.Amount;
                _dbContext.Entry(order).Property(x => x.DispatcherId).IsModified = true;
                _dbContext.Entry(order).Property(x => x.DeliveryFee).IsModified = true;

                await _dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                return SuccessResult();
            }
            catch (Exception ex)
            {
               await trans.RollbackAsync();
                return ServerExceptionError(ex);
            }
        }
    }
}
