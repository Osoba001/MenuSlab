using Main.Application.Requests.CountryRequests;
using Main.Application.Requests.PaymentGatewayRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.PaymentMethodHandlers
{
    internal class FetchPaymentGatewayHandler(MenuslabDbContext dbContext) : IRequestHandler<FetchPaymentGatewayRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(FetchPaymentGatewayRequest request, CancellationToken cancellationToken = default)
        {
            return new ActionResponse
            {
                PayLoad = await _dbContext.PaymentGateways.Select(x => new PaymentGatewayResponse
                {
                    Name = x.Name,
                    Countries = x.Countries.Select(x => new CountryResponse { Currency = x.Currency, Name = x.Name })
                }).ToListAsync()
            };
        }
    }
}
