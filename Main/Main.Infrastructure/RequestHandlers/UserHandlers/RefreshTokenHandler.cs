using Main.Application.Requests.UserRequests;
using Main.Infrastructure.Authentications;
using Main.Infrastructure.Database;

namespace Main.Infrastructure.RequestHandlers.UserHandlers
{
    internal class RefreshTokenHandler(IAuthService authServices, MenuslabDbContext dbContext) : IRequestHandler<RefreshTokenRequest>
    {
        private readonly IAuthService _authServices = authServices;
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var id = _authServices.RetrieveTokenNameIdentifier(request.AccessToken);
            if (id == null)
                return BadRequestResult(InvalidToken);
            var user = await _dbContext.Users.FindAsync(id.Value);
            if (user is null)
                return BadRequestResult(InvalidToken);

            if (request.RefreshToken != user.RefreshToken)
                return BadRequestResult(InvalidToken);
            if (DateTime.UtcNow > user.RefreshTokenExpireTime)
                return BadRequestResult(InvalidToken);

            TokenModel token = new() { Email = user.Email, Name = user.Name, Role = user.Role, Id = user.Id, Session=Guid.NewGuid().ToString("N") };
            _authServices.GenerateTokens(token);
            user.RefreshToken = token.RefreshToken;
            user.RefreshTokenExpireTime = token.RefreshTokenExpireTime;
            user.Session = token.Session;
            _dbContext.Users.Entry(user).Property(x => x.Session).IsModified = true;
            _dbContext.Users.Entry(user).Property(x => x.RefreshToken).IsModified = true;
            _dbContext.Users.Entry(user).Property(x => x.RefreshTokenExpireTime).IsModified = true;
            await _dbContext.CompleteAsync();
            return new ActionResponse
            {
                PayLoad = token
            };
        }
    }
}
