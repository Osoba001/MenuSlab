using Main.Application.Requests.RestaurantStaffRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.RestaurantStaffHandlers
{
    internal class UpdateRestaurantStaffHandler(MenuslabDbContext dbContext) : IRequestHandler<UpdateRestaurantStaffRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(UpdateRestaurantStaffRequest request, CancellationToken cancellationToken = default)
        {
            var restaurant = await _dbContext.Restaurants.Where(x => x.Id == request.RestaurantId && x.UserId == request.UserIdentifier)
                .FirstOrDefaultAsync();
            if (restaurant == null) return NotFoundResult();

            var eStaff = await _dbContext.RestaurantStaff.Where(x => x.RestaurantId == request.RestaurantId && x.Id==request.Id)
                .FirstOrDefaultAsync();
            if (eStaff == null) return NotFoundResult();

            var change = new { eStaff.StartOfDuty, eStaff.EndOfDuty, eStaff.Role, eStaff.StaffId, eStaff.IsActive };

            eStaff.StartOfDuty = request.StartOfDuty;
            eStaff.EndOfDuty = request.EndOfDuty;
            eStaff.Role = request.Role;
            eStaff.StaffId = request.StaffId;
            eStaff.IsActive = request.IsActive;

            _dbContext.Entry(eStaff).Property(x => x.StartOfDuty).IsModified = change.StartOfDuty != request.StartOfDuty;
            _dbContext.Entry(eStaff).Property(x => x.EndOfDuty).IsModified = change.EndOfDuty != request.EndOfDuty;
            _dbContext.Entry(eStaff).Property(x => x.Role).IsModified = change.Role != request.Role;
            _dbContext.Entry(eStaff).Property(x => x.StaffId).IsModified = change.StaffId != request.StaffId;
            _dbContext.Entry(eStaff).Property(x => x.IsActive).IsModified = change.IsActive != request.IsActive;

            return await _dbContext.CompleteAsync();
        }
    }
}
