using Main.Application.Requests.FundTransactionRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.FundTransactionHandlers
{
    internal class FetchTransactionHandler(MenuslabDbContext dbContext):IRequestHandler<FetchTransactionRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(FetchTransactionRequest request, CancellationToken cancellationToken = default)
        {
            if (request.Page <= 0 || request.PageSize <= 0)
                return BadRequestResult("Invalid Page or PageSize values.");

            int offset = (request.Page - 1) * request.PageSize;

            return new ActionResponse
            {
                PayLoad = await _dbContext.Funds.Where(x => x.SenderId == request.UserIdentifier || x.ReceiverId == request.UserIdentifier)
                .OrderByDescending(x => x.CreatedDate)
                .Skip(offset).Take(request.PageSize)
                .Select(x => new TransactionResponse
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    Sender = x.Sender != null ? x.Sender.Name : null,
                    SenderId = x.SenderId,
                    ReceiverId = x.ReceiverId,
                    Receiver= x.Receiver != null ? x.Receiver.Name : null,
                    OrderId = x.OrderId,
                    CreatedDate = x.CreatedDate,
                    FundTransactionType=x.FundTransactionType.ToString(),
                    TransactionId=x.TransactionId,
                    Restaurant=x.Order!=null?x.Order.Restaurant.Name : null,
                    IsCredit=x.ReceiverId==request.UserIdentifier,
                    
                }).ToListAsync()
            };
        }
    }
}
