using Main.Application.Requests.UserRequests;
using Main.Infrastructure.Authentications;
using Main.Infrastructure.Database;


namespace Main.Infrastructure.RequestHandlers.UserHandlers
{
    internal class ChangePasswordCommandHandler(IAuthService services, MenuslabDbContext context) : IRequestHandler<ChangePasswordRequest>
    {
        private readonly IAuthService _authServices = services;
        private readonly MenuslabDbContext _dbContext = context;

        public async Task<ActionResponse> HandleAsync(ChangePasswordRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _dbContext.Users.FindAsync(request.UserIdentifier);

            if (user == null)
                return NotFoundResult(UserNotFound);

            if (user.IsLock)
            {
                if (user.Attempt > 5)
                {
                    return Unauthorized($"Account is lock. \n {user.LockMessage}");
                }
                if (user.WhenToUnlockInMinutes > DateTime.UtcNow)
                {
                    var waitingTime = (user.WhenToUnlockInMinutes - DateTime.UtcNow).TotalMinutes;
                    return Unauthorized($"Account is lock for the nest {waitingTime}. \n {user.LockMessage}");
                }
                user.IsLock = false;
                _dbContext.Users.Entry(user).Property(x => x.IsLock).IsModified = true;
                await _dbContext.CompleteAsync();
            }


            if (!Database.Entities.User.VerifyPassword(request.OldPassword, user.PasswordHash))
            {

                user.Attempt++;
                if (user.Attempt > 4)
                {
                    user.IsLock = true;
                    user.LockMessage = "Too many attent";
                    user.Attempt = 0;
                    user.WhenToUnlockInMinutes = DateTime.UtcNow.AddMinutes(30);
                    _dbContext.Users.Entry(user).Property(x => x.IsLock).IsModified = true;
                    _dbContext.Users.Entry(user).Property(x => x.LockMessage).IsModified = true;
                    _dbContext.Users.Entry(user).Property(x => x.WhenToUnlockInMinutes).IsModified = true;

                }
                _dbContext.Users.Entry(user).Property(x => x.Attempt).IsModified = true;
                await _dbContext.CompleteAsync();
                return Unauthorized(InvalidPassword);
            }

            user!.PasswordHash = Database.Entities.User.HashPassword(request.NewPassword);

            _dbContext.Users.Entry(user).Property(x => x.PasswordHash).IsModified = true;
            return await _dbContext.CompleteAsync();
        }
    }
}