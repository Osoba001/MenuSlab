using Main.Application.Enums;
using Main.Application.Requests.UserRequests;
using Main.Infrastructure.Authentications;
using Main.Infrastructure.Database;


namespace Main.Infrastructure.RequestHandlers.UserHandlers
{
    internal class DeleteUserHandler(MenuslabDbContext context, IAuthService authServices) : IRequestHandler<DeleteUserRequest>
    {
        private readonly MenuslabDbContext _context = context;
        private readonly IAuthService _authServices = authServices;

        public async Task<ActionResponse> HandleAsync(DeleteUserRequest request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(request.UserIdentifier);
            if (user is null)
                return NotFoundResult(UserNotFound);
            if (user.AuthenticationType == AuthenticationType.EmailPassWord.ToString())
                if (!Database.Entities.User.VerifyPassword(request.Password, user.PasswordHash))
                    return BadRequestResult("Incorrect password.");
            

            _context.Users.Remove(user);
            var result = await _context.CompleteAsync();
            if (result.IsSuccess)
            {
                result.Information = $"Action: Deleted {user.Name} User,\n Reason {request.Reason},\n email: {user.Email},\n Role: {user.Role},\n Id: {user.Id},\n Country: {user.CountryId}";
                request.OnHardDelete(user.Id);
            }

            return result;
        }

    }
}
