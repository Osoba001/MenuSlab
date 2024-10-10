using Main.Application.Requests.RestaurantStaffRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.RestaurantStaffHandlers
{
    internal class RemoveRestaurantStaffHandler(MenuslabDbContext dbContext) : IRequestHandler<RemoveRestaurantStaffRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(RemoveRestaurantStaffRequest request, CancellationToken cancellationToken = default)
        {
            var restaurant = await _dbContext.Restaurants.Where(x => x.Id == request.RestaurantId && x.UserId == request.UserIdentifier)
               .FirstOrDefaultAsync();
            if (restaurant == null) return NotFoundResult();

            var existStaffs = await _dbContext.RestaurantStaff.Where(x => x.RestaurantId == request.RestaurantId && request.Ids.Contains(x.Id))
                .ToDictionaryAsync(x => x.UserId, x => x);
            int success = 0;
            foreach (var id in request.Ids)
            {
                if (existStaffs.TryGetValue(id, out var eStaff))
                {
                    _dbContext.RestaurantStaff.Remove(eStaff);
                    success++;
                }
            }
            return await _dbContext.CompleteAsync(new { Result = $"{success} staff Removed" });
        }
    }
}
