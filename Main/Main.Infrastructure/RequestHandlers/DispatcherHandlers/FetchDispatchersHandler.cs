using Main.Application.Requests.DispatcherRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.DispatcherHandlers
{
    internal class FetchDispatchersHandler(MenuslabDbContext dbContext) : IRequestHandler<FetchDispatchersRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(FetchDispatchersRequest request, CancellationToken cancellationToken = default)
        {
            if (request.Page <= 0 || request.PageSize <= 0)
                return BadRequestResult("Invalid Page or PageSize values.");

            int offset = (request.Page - 1) * request.PageSize;

            return new ActionResponse
            {
                PayLoad = await _dbContext.Dispatchers.Where(x => x.CountryId == request.Country && x.Number.StartsWith(request.SubNumber))
               .Select(x => new FetchDispatcherResponse
               {
                   Country = x.CountryId,
                   Name = x.User.Name,
                   Number = x.Number,
                   PhoneNo = x.PhoneNo,
                   Rider = x.Rider,
                   Id = x.Id,
                   IsActive = x.IsActive,
               }).OrderBy(x => x.Number)
               .Skip(offset)
                .Take(request.PageSize)
                .ToListAsync()
            };

        }
    }
}
