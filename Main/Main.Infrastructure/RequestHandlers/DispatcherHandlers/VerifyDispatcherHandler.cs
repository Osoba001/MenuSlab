using Main.Application.Requests.DispatcherRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.DispatcherHandlers
{
    internal class VerifyDispatcherHandler(MenuslabDbContext dbContext) : IRequestHandler<VerifyDispatcherRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(VerifyDispatcherRequest request, CancellationToken cancellationToken = default)
        {
            var dispatchers = await _dbContext.Dispatchers.Where(x => request.Ids.Contains(x.Id)).ToListAsync();
            foreach (var dispatcher in dispatchers)
            {
                dispatcher.Number = request.Number;
                _dbContext.Dispatchers.Entry(dispatcher).Property(property => property.Number).IsModified=true;
            }
            return await _dbContext.CompleteAsync();

        }
    }
}
