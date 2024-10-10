using Main.Application.Enums;
using Main.Application.Requests.OrderRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.OrderHandlers
{
    internal class FetchOrderByDispatcherHandler(MenuslabDbContext dbContext) : IRequestHandler<FetchOrderByDispatcherRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(FetchOrderByDispatcherRequest request, CancellationToken cancellationToken = default)
        {
            if (request.Page <= 0 || request.PageSize <= 0)
                return BadRequestResult("Invalid Page or PageSize values.");

            int offset = (request.Page - 1) * request.PageSize;

            return new ActionResponse
            {
                PayLoad = await _dbContext.Orders.Where(x => x.DispatcherId == request.DispatcherId)
                .OrderByDescending(x => x.CreatedDate)
                .Skip(offset).Take(request.PageSize)
                .Select(x => new FetchOrderByDispatcherResponse
                {
                    DeleveryFee = x.DeliveryFee,
                    CreatedDate = x.CreatedDate,
                    RestaurantId = x.RestaurantId,
                    RestaurantName = x.Restaurant.Name,
                    ReceiverId = x.ReceiverId,
                    Receiver = x.Receiver!=null?x.Receiver.Name:null,
                    Status = x.Status,
                    UserId = x.UserId,
                    UserName = x.User.Name,
                    Id = x.Id,
                    DeliveryCoordinate=x.DeliveryCoordinates,
                    OrderNumber = x.OrderNumber,
                    DeliveryPaid = x.Payments.Where(x=>x.FundTransactionType==FundTransactionType.PayForDelivery).Sum(x=>x.Amount),
                })
                .ToListAsync()

            };
        }
    }
}
