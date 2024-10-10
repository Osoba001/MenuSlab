using Main.Application.Requests.UserRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;


namespace Main.Infrastructure.RequestHandlers.UserHandlers
{
    internal class FetchUsersHandler(MenuslabDbContext dbContext) : IRequestHandler<FetchUsersRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(FetchUsersRequest request, CancellationToken cancellationToken)
        {
            return new ActionResponse
            {
                PayLoad = await _dbContext.Users.Select(u => new UserResponse
                {
                    Email = u.Email,
                    Role = u.Role,
                    Id = u.Id,
                    CountryId = u.CountryId,
                    Name = u.Name,
                    PhoneNo = u.PhoneNo,
                }).AsNoTracking()
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync()
            };
        }

    }
}
