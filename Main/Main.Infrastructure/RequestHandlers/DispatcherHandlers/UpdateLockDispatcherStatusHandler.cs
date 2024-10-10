using Main.Application.Requests.DispatcherRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.DispatcherHandlers
{
    internal class UpdateLockDispatcherStatusHandler(MenuslabDbContext dbContext) : IRequestHandler<UpdateLockDispatcherStatusRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(UpdateLockDispatcherStatusRequest request, CancellationToken cancellationToken = default)
        {
            var dispatcher = await _dbContext.Dispatchers.Where(x => x.Id == request.Id).FirstOrDefaultAsync();
            if (dispatcher == null) return NotFoundResult();

            dispatcher.IsActive = request.IsActive;
            dispatcher.ActiveMessage = request.Message;

            _dbContext.Dispatchers.Entry(dispatcher).Property(x => x.IsActive).IsModified = true;
            _dbContext.Dispatchers.Entry(dispatcher).Property(x => x.ActiveMessage).IsModified = true;
            return await _dbContext.CompleteAsync(cancellationToken);
        }
    }
}
