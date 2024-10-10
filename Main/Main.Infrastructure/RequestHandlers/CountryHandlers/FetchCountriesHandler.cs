using Main.Application.Requests.CountryRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.CountryHandlers
{
    internal class FetchCountriesHandler(MenuslabDbContext dbContext) : IRequestHandler<FetchCountriesRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(FetchCountriesRequest request, CancellationToken cancellationToken = default)
        {
            return new ActionResponse { PayLoad= await _dbContext.Countries.Select(x => new CountryResponse { Name = x.Name, Currency = x.Currency }).ToListAsync() };
        }
    }

    internal class FetchCountriesFullDetailHandler(MenuslabDbContext dbContext) : IRequestHandler<FetchCountriesFullDetailRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(FetchCountriesFullDetailRequest request, CancellationToken cancellationToken = default)
        {
            return new ActionResponse { PayLoad = await _dbContext.Countries.Select(x => new CountryFullDetailResponse
            { 
                Name = x.Name, 
                Currency = x.Currency,
                MinWithDrawal = x.MinWithDrawal,
                BaseOrderChange = x.BaseOrderChange,
                MaxTransfer = x.MaxTransfer,
                MaxWithDrawal   = x.MaxWithDrawal,
                MinTransfer = x.MinTransfer,
                PaymentGatewayId = x.PaymentGatewayId,
                PaymentGateway=x.PaymentGateway!=null?x.PaymentGateway.Name:null,
            }).ToListAsync() };
        }
    }
}
