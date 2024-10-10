using Main.Application.Requests.UserRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;


namespace Main.Infrastructure.RequestHandlers.UserHandlers
{
    internal class UserByEmailQueryHandler(MenuslabDbContext dbContext) : IRequestHandler<UserByEmailRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(UserByEmailRequest request, CancellationToken cancellationToken)
        {
            var resp = await _dbContext.Users.Where(x => x.Email == request.Email.ToLower()).FirstOrDefaultAsync();
            if (resp != null)
            {
                UserResponse user = resp;
                return new ActionResponse
                {
                    PayLoad = user
                };
            }
            return NotFoundResult(UserNotFound);
        }
    }
}
