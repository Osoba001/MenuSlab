using Main.Application.Requests.PaymentGatewayRequests;
using Main.Infrastructure.Database;

namespace Main.Infrastructure.RequestHandlers.PaymentMethodHandlers
{
    internal class AddPaymentGatewayHandler(MenuslabDbContext dbContext) : IRequestHandler<AddPaymentGatewayRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(AddPaymentGatewayRequest request, CancellationToken cancellationToken = default)
        {
            _dbContext.PaymentGateways.Add(new Database.Entities.PaymentGateway { Name = request.Name, ImplementationName=request.ImplementationName });
            return await _dbContext.CompleteAsync();
        }
    }
}
