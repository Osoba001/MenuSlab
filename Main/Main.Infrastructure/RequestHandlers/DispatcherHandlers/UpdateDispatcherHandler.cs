using Main.Application.Requests.DispatcherRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.DispatcherHandlers
{
    internal class UpdateDispatcherHandler(MenuslabDbContext dbContext) : IRequestHandler<UpdateDispatcherRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(UpdateDispatcherRequest request, CancellationToken cancellationToken = default)
        {
            var dispatcher = await _dbContext.Dispatchers.Where(x => x.UserId == request.UserIdentifier).FirstOrDefaultAsync();
            if (dispatcher == null) return NotFoundResult();

            dispatcher.PhoneNo = request.PhoneNo;
            dispatcher.Rider = request.Rider;

            _dbContext.Dispatchers.Entry(dispatcher).Property(x=>x.PhoneNo).IsModified=true;
            _dbContext.Dispatchers.Entry(dispatcher).Property(x=>x.Rider).IsModified=true;
            return await _dbContext.CompleteAsync(cancellationToken);
        }
    }
}
