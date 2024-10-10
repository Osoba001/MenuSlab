using Main.Application.Requests.SystemAdminRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.SystemAdminHandlers
{
    internal class UpdateSystemAdminHandler(MenuslabDbContext dbContext) : IRequestHandler<UpdateSystemAdminRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(UpdateSystemAdminRequest request, CancellationToken cancellationToken = default)
        {
            var staff = await _dbContext.SystemStaff.FindAsync(request.Id);
            if (staff == null) return NotFoundResult();

           var admin= await _dbContext.SystemStaff
                .Where(x=>x.UserId==request.UserIdentifier && x.CountryId==staff.CountryId && (x.Role!=Application.Enums.StaffRole.Staff)).FirstAsync();
            if (admin == null) return Forbiden();

            var change= new {staff.StaffNumber, staff.Role, staff.IsActive};

            staff.StaffNumber = request.StaffNumber;
            staff.Role= request.Role;
            staff.IsActive= request.IsActive;

            _dbContext.Entry(staff).Property(x => x.Role).IsModified = change.Role!=request.Role;
            _dbContext.Entry(staff).Property(x => x.StaffNumber).IsModified = change.StaffNumber!=request.StaffNumber;
            _dbContext.Entry(staff).Property(x => x.IsActive).IsModified = change.IsActive!=request.IsActive;

            return await _dbContext.CompleteAsync(cancellationToken);
        }
    }
   
}
