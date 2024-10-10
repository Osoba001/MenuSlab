using Main.Application.Requests.RestaurantStaffRequests;
using Main.Infrastructure.Database;
using Main.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.RestaurantStaffHandlers
{
    internal class AddRestaurantStaffHandler(MenuslabDbContext dbContext) : IRequestHandler<AddRestaurantStaffRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(AddRestaurantStaffRequest request, CancellationToken cancellationToken = default)
        {
            var restaurant= await _dbContext.Restaurants.Where(x=>x.Id==request.RestaurantId && x.UserId==request.UserIdentifier)
                .FirstOrDefaultAsync();
            if (restaurant == null) return NotFoundResult();

            var existStaffs= await _dbContext.RestaurantStaff.Where(x=>x.RestaurantId==request.RestaurantId)
                .ToDictionaryAsync(x=>x.UserId, x=>x);
            int success= 0;
            foreach (var staff in request.Staffs)
            {
                if(!existStaffs.TryGetValue(staff.UserId, out var eStaff))
                {
                    eStaff = new RestaurantStaff
                    {
                        EndOfDuty = staff.EndOfDuty,
                        StartOfDuty = staff.StartOfDuty,
                        RestaurantId = request.RestaurantId,
                        Role = staff.Role,
                        StaffId = staff.StaffId,
                        UserId = staff.UserId
                    };
                    _dbContext.RestaurantStaff.Add(eStaff);
                    success++;
                }
               
            }
            return await _dbContext.CompleteAsync(new { Result=$"{success} staff added" });
        }
    }
}
