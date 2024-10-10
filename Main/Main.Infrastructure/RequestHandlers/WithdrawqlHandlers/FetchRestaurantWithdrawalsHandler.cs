using Main.Application.Requests.WithdrawalRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.WithdrawqlHandlers
{
    internal class FetchRestaurantWithdrawalsHandler(MenuslabDbContext dbContext) : IRequestHandler<FetchRestaurantWithdrawalsRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(FetchRestaurantWithdrawalsRequest request, CancellationToken cancellationToken = default)
        {
            if (request.Page <= 0 || request.PageSize <= 0)
                return BadRequestResult("Invalid Page or PageSize values.");

            int offset = (request.Page - 1) * request.PageSize;
            return new ActionResponse
            {
                PayLoad = await _dbContext.WithdrewFunds.Where(x => x.RestaurantId == request.RestaurantId)
                .OrderByDescending(x => x.RequestedDate)
                .Skip(offset).Take(request.PageSize)
                .Select(x => new WithdrawalDispatcherResponse
                {
                    Id = x.Id,
                    RequestedDate = x.RequestedDate,
                    RespondedDate = x.RespondedDate,
                    Amount = x.Amount,
                    Charges = x.Amount,
                    NetBalance = x.NetBalance,
                    CountryId = x.CountryId,
                    Message = x.Message,
                    WithdrawStatus = x.WithdrawStatus.ToString(),

                }).ToListAsync()
            };
        }
    }
}
