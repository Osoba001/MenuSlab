using Main.Application.Requests.DispatcherRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.DispatcherHandlers
{
    internal class DeleteDispatcherHandler(MenuslabDbContext dbContext) : IRequestHandler<DeleteDispatcherRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(DeleteDispatcherRequest request, CancellationToken cancellationToken = default)
        {
            var dispatcher = await _dbContext.Dispatchers.Where(x => x.UserId == request.UserIdentifier).FirstOrDefaultAsync();
            if (dispatcher == null) return NotFoundResult();

            _dbContext.Dispatchers.Remove(dispatcher);
            return await _dbContext.CompleteAsync();
        }
    }
}
