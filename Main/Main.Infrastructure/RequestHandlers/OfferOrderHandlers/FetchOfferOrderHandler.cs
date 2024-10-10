using Main.Application.Enums;
using Main.Application.Requests.OfferOrderRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.OfferOrderHandlers
{
    internal class FetchOfferOrderHandler(MenuslabDbContext dbContext) : IRequestHandler<FetchOfferOrderRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(FetchOfferOrderRequest request, CancellationToken cancellationToken = default)
        {
            return new ActionResponse
            {
                PayLoad = await _dbContext.OfferOrders.Where(x => (x.UserId == request.UserIdentifier && x.Delete!=DeleteStatus.Sender )|| (x.ReceiverId == request.UserIdentifier && x.Delete != DeleteStatus.Receiver))
                .OrderByDescending(x => x.CreatedDate)
                .Take(20)
                .Select(x => new FetchOfferOrderResponse
                {
                    Code = x.Code,
                    Status = x.OfferOderStatus.ToString(),
                    CreatedDate = x.CreatedDate,
                    Sender = x.User.Name,
                    SenderId = x.UserId,
                    Receiver = x.Receiver==null?null:x.Receiver.Name,
                    ReceiverId = x.ReceiverId,
                    DeliveryCoordinates = x.DeliveryCoordinates,
                    Id = x.Id,
                }).ToListAsync()
            };
        }
    }
}
