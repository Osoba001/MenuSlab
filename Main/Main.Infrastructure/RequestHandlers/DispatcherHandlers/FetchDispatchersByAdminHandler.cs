using Main.Application.Requests.DispatcherRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.DispatcherHandlers
{
    internal class FetchDispatchersByAdminHandler(MenuslabDbContext dbContext) : IRequestHandler<FetchDispatchersByAdminRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(FetchDispatchersByAdminRequest request, CancellationToken cancellationToken = default)
        {
            if (request.Page <= 0 || request.PageSize <= 0)
                return BadRequestResult("Invalid Page or PageSize values.");

            int offset = (request.Page - 1) * request.PageSize;

            return new ActionResponse
            {
                PayLoad = await _dbContext.Dispatchers.Where(x =>x.CountryId == request.Country)
                .Select(x=> new FetchFullDispatcherResponse
                {
                    Country=x.CountryId,
                    Name=x.User.Name,
                    Number=x.Number,
                    PhoneNo=x.PhoneNo,
                    Rider=x.Rider,
                    Id=x.Id,
                    IsActive = x.IsActive,
                    CreatedDate =x.CreatedDate,
                    Balance=x.Balance,
                    NetBalance=x.NetBalance,
                    Charge=x.Charges,
                }).OrderByDescending(x=>x.CreatedDate)
                .Skip(offset)
                .Take(request.PageSize)
                .ToListAsync()
            };
        }
    }
}
