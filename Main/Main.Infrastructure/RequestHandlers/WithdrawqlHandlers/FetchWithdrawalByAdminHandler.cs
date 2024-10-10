using Main.Application.Requests.WithdrawalRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.WithdrawqlHandlers
{
    internal class FetchWithdrawalByAdminHandler(MenuslabDbContext dbContext) : IRequestHandler<FetchtWithdrawalByAdminRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(FetchtWithdrawalByAdminRequest request, CancellationToken cancellationToken = default)
        {
            if (request.Page <= 0 || request.PageSize <= 0)
                return BadRequestResult("Invalid Page or PageSize values.");

            int offset = (request.Page - 1) * request.PageSize;

            return new ActionResponse
            {
                PayLoad = await _dbContext.WithdrewFunds.Where(x => x.CountryId==request.Country && x.SourceType==request.SourceType && x.Number.StartsWith(request.SubNumber))
                .OrderByDescending(x => x.RequestedDate)
                .Skip(offset).Take(request.PageSize)
                .Select(x => new WithdrawalByAdminResponse
                {
                    Id = x.Id,
                    RequestedDate = x.RequestedDate,
                    RespondedDate = x.RespondedDate,
                    Amount = x.Amount,
                    Charges = x.Amount,
                    NetBalance = x.NetBalance,
                    Message = x.Message,
                    WithdrawStatus = x.WithdrawStatus.ToString(),
                    Number = x.Number,
                    SourceType=x.SourceType.ToString(),
                    SystemAdmin=x.SystemAdmin!=null? x.SystemAdmin.User.Name:null,
                    StaffNumber=x.SystemAdmin!=null? x.SystemAdmin.StaffNumber:null,
                    SystemAdminId=x.SystemAdminId,
                }).ToListAsync()
            };
        }
    }
}
