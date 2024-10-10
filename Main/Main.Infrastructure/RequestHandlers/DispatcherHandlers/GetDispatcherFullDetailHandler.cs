using Main.Application.Requests.DispatcherRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.DispatcherHandlers
{
    internal class GetDispatcherFullDetailHandler(MenuslabDbContext dbContext) : IRequestHandler<GetDispatcherFullDetailRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(GetDispatcherFullDetailRequest request, CancellationToken cancellationToken = default)
        {
            var dispatcher = await _dbContext.Dispatchers.Where(x => x.Id == request.Id)
               .Select(x => new GetDispatcherFullDetailResponse
               {
                   Country = x.CountryId,
                   Name = x.User.Name,
                   Currency = x.Country.Currency,
                   Number = x.Number,
                   PhoneNo = x.PhoneNo,
                   Rider = x.Rider,
                   Id = x.Id,
                   BankAccount=x.BankDetails,
                   CreatedDate = x.CreatedDate,
                   IsActive = x.IsActive,
                   ActiveMessage=x.ActiveMessage,
                   Charge=x.Charges,
                   NetBalance=x.NetBalance,
                   Balance=x.Balance,
               }).FirstOrDefaultAsync();
            if (dispatcher == null) return NotFoundResult();
            return new ActionResponse { PayLoad = dispatcher };
        }
    }
}
