using Main.Application.Enums;
using Main.Application.Requests.OfferOrderRequests;
using Main.Infrastructure.Database;
using Main.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.OfferOrderHandlers
{
    internal class RespondOfferOrderHandler(MenuslabDbContext dbContext) : IRequestHandler<RespondOfferOrderRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(RespondOfferOrderRequest request, CancellationToken cancellationToken = default)
        {
            OfferOrder? offer= null;
            if(request.Id!=null) 
                offer= await _dbContext.OfferOrders.FindAsync(request.Id);
            else
            {
                offer = await _dbContext.OfferOrders.Where(x => x.Code == request.Code && x.CreatedDate > DateTime.UtcNow.AddHours(24)).FirstOrDefaultAsync();
                if(offer!=null)
                {
                    offer.ReceiverId = request.UserIdentifier;
                    
                }    
            }

            if (offer == null)
                return NotFoundResult("This offer is no longer.");

            offer.OfferOderStatus=request.IsAccepted?OfferOderStatus.Accepted:OfferOderStatus.Declined;
            _dbContext.OfferOrders.Update(offer);

            return await _dbContext.CompleteAsync();
        }
    }
}
