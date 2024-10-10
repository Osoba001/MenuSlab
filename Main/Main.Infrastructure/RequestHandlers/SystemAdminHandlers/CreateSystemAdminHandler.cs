using Main.Application.Requests.SystemAdminRequests;
using Main.Infrastructure.Database;
using Main.Infrastructure.Database.Entities;

namespace Main.Infrastructure.RequestHandlers.SystemAdminHandlers
{
    internal class CreateSystemAdminHandler(MenuslabDbContext dbContext) : IRequestHandler<CreateSystemAdminRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(CreateSystemAdminRequest request, CancellationToken cancellationToken = default)
        {
            var admin= new SystemAdmin 
            {
                CountryId=request.CountryId,
                UserId=request.UserId,
                StaffNumber=request.StaffNumber,
                Role=request.Role,
            };
            _dbContext.SystemStaff.Add(admin);
            return await _dbContext.CompleteAsync(new { admin.Id });
        }
    }
   
}
