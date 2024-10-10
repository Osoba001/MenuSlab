using Main.Application.Requests.DispatcherRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.DispatcherHandlers
{
    internal class GetDispatcherDetailHandler(MenuslabDbContext dbContext) : IRequestHandler<GetDispatcherDetailRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(GetDispatcherDetailRequest request, CancellationToken cancellationToken = default)
        {
            var dispatcher= await _dbContext.Dispatchers.Where(x=>x.Id==request.Id)
                .Select(x=> new GetDispatcherDetailResponse
                {
                    Country=x.CountryId, 
                    Name = x.User.Name,
                    Currency=x.Country.Currency,
                    Number = x.Number,
                    PhoneNo = x.PhoneNo,
                    Rider = x.Rider,
                    Id = x.Id,
                }).FirstOrDefaultAsync();
            if (dispatcher == null) return NotFoundResult();
            return new ActionResponse { PayLoad = dispatcher };
        }
    }
}
