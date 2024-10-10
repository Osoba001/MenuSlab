using Main.Application.Requests.UserRequests;
using Main.Infrastructure.Database;

namespace Main.Infrastructure.RequestHandlers.UserHandlers
{
    internal class UpdateUserDetailsHandler(MenuslabDbContext dbContext) : IRequestHandler<UpdateUserDetailsRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(UpdateUserDetailsRequest request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FindAsync(request.UserIdentifier, cancellationToken);
            if (user is null)
                return BadRequestResult("User not found");

            user.Name = request.Name;
            user.PhoneNo = request.Phone;

            _dbContext.Users.Entry(user).Property(x => x.Name).IsModified = true;
            _dbContext.Users.Entry(user).Property(x => x.PhoneNo).IsModified = true;

            return await _dbContext.CompleteAsync();
        }
    }
}
