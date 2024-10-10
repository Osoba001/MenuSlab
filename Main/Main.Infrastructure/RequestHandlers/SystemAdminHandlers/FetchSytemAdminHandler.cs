using Main.Application.Requests.SystemAdminRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.SystemAdminHandlers
{
    internal class FetchSytemAdminHandler(MenuslabDbContext dbContext) : IRequestHandler<FetchSytemAdminRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(FetchSytemAdminRequest request, CancellationToken cancellationToken = default)
        {
            return new ActionResponse
            {
                PayLoad = await _dbContext.SystemStaff.Where(x => x.CountryId == request.CountryId)
                .Select(x => new FetchSytemAdminResponse
                {
                    Country = x.CountryId,
                    Name = x.User.Name,
                    StaffNumber = x.StaffNumber,
                    Role = x.Role,
                    Id = x.Id,
                    IsActive = x.IsActive,
                    JoinDate = x.JoinDate,
                    UserId = x.UserId,
                }).ToListAsync()
            };
        }
    }
   
}
