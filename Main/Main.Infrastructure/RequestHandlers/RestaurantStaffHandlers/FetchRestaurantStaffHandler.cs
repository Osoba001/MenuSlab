using Main.Application.Requests.RestaurantStaffRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.RestaurantStaffHandlers
{
    internal class FetchRestaurantStaffHandler(MenuslabDbContext dbContext) : IRequestHandler<FetchRestaurantStaffRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(FetchRestaurantStaffRequest request, CancellationToken cancellationToken = default)
        {
            return new ActionResponse
            {
                PayLoad = await _dbContext.RestaurantStaff.Where(x => x.RestaurantId == request.RestaurantId)
                .Select(x => new RestaurantStaffResponse
                {
                    Id = x.Id,
                    EndOfDuty = x.EndOfDuty,
                    StaffId = x.StaffId,
                    UserId = x.UserId,
                    Name = x.User.Name,
                    StartOfDuty = x.StartOfDuty,
                    IsActive = x.IsActive,
                    Role = x.Role,
                    PhoneNo = x.User.PhoneNo,
                }).ToListAsync()
            };
        }
    }
}
