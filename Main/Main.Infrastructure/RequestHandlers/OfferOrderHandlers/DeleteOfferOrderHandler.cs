using Main.Application.Enums;
using Main.Application.Requests.OfferOrderRequests;
using Main.Infrastructure.Database;

namespace Main.Infrastructure.RequestHandlers.OfferOrderHandlers
{
    internal class DeleteOfferOrderHandler(MenuslabDbContext dbContext) : IRequestHandler<DeleteOfferOrderRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(DeleteOfferOrderRequest request, CancellationToken cancellationToken = default)
        {
            var offer = await _dbContext.OfferOrders.FindAsync(request.Id);
            if (offer == null) return NotFoundResult();
            if(offer.Delete==DeleteStatus.Sender || offer.Delete==DeleteStatus.Receiver)
            {
                _dbContext.OfferOrders.Remove(offer);
                return await _dbContext.CompleteAsync(cancellationToken);
            }
            if(offer.ReceiverId == request.UserIdentifier)
                offer.Delete = DeleteStatus.Receiver;
            else
                offer.Delete=DeleteStatus.Sender;
            return await _dbContext.CompleteAsync();
        }
    }
}
