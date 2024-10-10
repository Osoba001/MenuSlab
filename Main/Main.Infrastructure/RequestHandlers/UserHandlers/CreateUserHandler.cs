using Main.Application.Enums;
using Main.Application.Requests.UserRequests;
using Main.Infrastructure.Authentications;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.UserHandlers
{
    internal class CreateUserHandler(IAuthService services, MenuslabDbContext context) : IRequestHandler<CreateUserRequest>
    {
        private readonly IAuthService _authServices = services;
        private readonly MenuslabDbContext _dbContext = context;

        public async Task<ActionResponse> HandleAsync(CreateUserRequest request, CancellationToken cancellationToken)
        {
            string email = request.Email.ToLower().Trim();
            var user = await _dbContext.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
            if (user is not null)
                return BadRequestResult(EmailAlreadyExist);

            user = new()
            {
                Email = email,
                Role = Role.User.ToString(),
                Name = request.Name,
                PhoneNo = request.PhoneNo,
                CountryId = request.Country,
                AuthenticationType = AuthenticationType.EmailPassWord.ToString(),
                PasswordHash = Database.Entities.User.HashPassword(request.Password),
                UserDevice= $"Device Type: {request.UserDevice.DeviceType}, Brand: {request.UserDevice.Brand}, Model: {request.UserDevice.Model}",
            };

            _dbContext.Users.Add(user);
            var res = await _dbContext.CompleteAsync();
            if (!res.IsSuccess)
                return res;

            TokenModel tokenModel = new() { Email=user.Email, Name=user.Name, Role=user.Role, Id=user.Id, Session=Guid.NewGuid().ToString("N") };
             _authServices.GenerateTokens(tokenModel);

            user.RefreshToken = tokenModel.RefreshToken;
            user.RefreshTokenExpireTime = tokenModel.RefreshTokenExpireTime;
            user.Session= tokenModel.Session;
            _dbContext.Users.Entry(user).Property(x => x.Session).IsModified = true;
            _dbContext.Users.Entry(user).Property(x => x.RefreshToken).IsModified = true;
            _dbContext.Users.Entry(user).Property(x => x.RefreshTokenExpireTime).IsModified = true;
            await _dbContext.CompleteAsync();
            return new ActionResponse
            {
                PayLoad = tokenModel
            };

        }


    }
}
