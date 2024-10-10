using Main.Application.Requests.PaymentGatewayRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.PaymentMethodHandlers
{
    internal class FetcBankHandler(MenuslabDbContext dbContext) : IRequestHandler<FetchBankRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(FetchBankRequest request, CancellationToken cancellationToken = default)
        {
            return new ActionResponse
            {
                PayLoad = await _dbContext.Banks.Where(x=>x.CountryId==request.Country).Select(x => new BankResponse { BankCode = x.BankCode, BankName = x.BankName }).ToListAsync()
            };
        }
    }
}
