using Main.Application.Requests.UserRequests;
using Main.Infrastructure.Authentications;
using Main.Infrastructure.Database;
using Main.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.UserHandlers
{
    internal class LoginHandler(IAuthService authServices, MenuslabDbContext dbContext) : IRequestHandler<LoginRequest>
    {
        private readonly IAuthService _authServices = authServices;
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            var user = await GetUserByEmailAsync(request.Email, cancellationToken);

            if (user is null)
                return Unauthorized("Invalid email or password.");

            if (user.IsLock)
            {
                var resp = await HandleLockLogic(user, cancellationToken);
                if (!resp.IsSuccess) return resp;
            }

            if (!User.VerifyPassword(request.Password, user.PasswordHash))
            {
                return await HandleAuthenticationFailure(user, cancellationToken);
            }

            return await GenerateTokenAndHandleRefreshToken(user, cancellationToken);
        }

        private async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email.ToLower().Trim(), cancellationToken);
        }

        private async Task<ActionResponse> HandleLockLogic(User user, CancellationToken cancellationToken)
        {
            //if (user.Attempt > 10)
            //{
            //    return Unauthorized($"This action is disable. \n {user.LockMessage}");
            //}
            //if (user.WhenToUnlock > DateTime.UtcNow)
            //{
            //    var waitingTime = (user.WhenToUnlock - DateTime.UtcNow).TotalMinutes;
            //    return Unauthorized($"This action is disable for the next {waitingTime} minutes. \n {user.LockMessage}");
            //}

            // Unlock the account
            user.IsLock = false;
            _dbContext.Users.Entry(user).Property(x => x.IsLock).IsModified = true;
            _dbContext.Users.Entry(user).Property(x => x.LockMessage).IsModified = true;
            return await _dbContext.CompleteAsync(cancellationToken);
        }

        private async Task<ActionResponse> HandleAuthenticationFailure(User user, CancellationToken cancellationToken)
        {
            var resp = Unauthorized("Invalid email or password.");
            user.Attempt++;

            if (user.Attempt > 9)
            {
                user.IsLock = true;
                user.LockMessage = "Due to too many login attempt.";
                user.Attempt = 0;
                user.WhenToUnlockInMinutes = DateTime.UtcNow.AddMinutes(15);

                _dbContext.Users.Entry(user).Property(x => x.IsLock).IsModified = true;
                _dbContext.Users.Entry(user).Property(x => x.LockMessage).IsModified = true;
                _dbContext.Users.Entry(user).Property(x => x.WhenToUnlockInMinutes).IsModified = true;
                resp.Information = $"Action: login {user.Name} User,\n Reason {user.LockMessage},\n email: {user.Email},\n Role: {user.Role},\n Id: {user.Id},\n Country: {user.CountryId}";
            }

            _dbContext.Users.Entry(user).Property(x => x.Attempt).IsModified = true;
            await _dbContext.CompleteAsync(cancellationToken);

            return resp;
        }

        private async Task<ActionResponse> GenerateTokenAndHandleRefreshToken(User user, CancellationToken cancellationToken)
        {

            TokenModel tokenModel = new() { Email = user.Email, Name = user.Name, Role = user.Role, Id = user.Id, Session=Guid.NewGuid().ToString("N") };
            _authServices.GenerateTokens(tokenModel);

            user.RefreshToken = tokenModel.RefreshToken;
            user.RefreshTokenExpireTime = tokenModel.RefreshTokenExpireTime;
            user.Session = tokenModel.Session;
            _dbContext.Users.Entry(user).Property(x => x.Session).IsModified = true;
            _dbContext.Users.Entry(user).Property(x => x.RefreshToken).IsModified = true;
            _dbContext.Users.Entry(user).Property(x => x.RefreshTokenExpireTime).IsModified = true;

            var res = await _dbContext.CompleteAsync(cancellationToken);
            if (!res.IsSuccess) return res;

            return new ActionResponse
            {
                PayLoad = tokenModel
            };
        }
    }
}
