using Main.Application.Requests.PaymentGatewayRequests;
using Main.Infrastructure.Database;

namespace Main.Infrastructure.RequestHandlers.PaymentMethodHandlers
{
    internal class AddBankHandler(MenuslabDbContext dbContext) : IRequestHandler<AddBankRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(AddBankRequest request, CancellationToken cancellationToken = default)
        {
            _dbContext.Banks.Add(new Database.Entities.Bank { BankCode= request.BankCode, BankName= request.BankName, CountryId=request.CountryId });
            return await _dbContext.CompleteAsync();
        }
    }
}
