using Main.Application.Requests.DispatcherRequests;
using Main.Infrastructure.Database;
using Main.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.DispatcherHandlers
{
    internal class CreateDispatcherHandler(MenuslabDbContext dbContext) : IRequestHandler<CreateDispatcherRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(CreateDispatcherRequest request, CancellationToken cancellationToken = default)
        {
            var dispatcher = await _dbContext.Dispatchers.Where(x => x.UserId == request.UserIdentifier).FirstOrDefaultAsync();
            if (dispatcher!= null) return BadRequestResult("Record already exist");

            dispatcher = new Dispatcher { CountryId = request.Country, PhoneNo = request.PhoneNo, Rider = request.Rider, UserId = request.UserIdentifier };
            _dbContext.Dispatchers.Add(dispatcher);
            return await _dbContext.CompleteAsync(dispatcher.Id);
        }
    }
}
