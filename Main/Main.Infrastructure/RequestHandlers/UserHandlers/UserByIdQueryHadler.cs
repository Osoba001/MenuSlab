using Main.Application.Requests.UserRequests;
using Main.Infrastructure.Database;


namespace Main.Infrastructure.RequestHandlers.UserHandlers
{
    internal class UserByIdQueryHadler(MenuslabDbContext authServices) : IRequestHandler<UserByIdRequest>
    {
        private readonly MenuslabDbContext _authServices = authServices;

        public async Task<ActionResponse> HandleAsync(UserByIdRequest request, CancellationToken cancellationToken)
        {
            var resp = await _authServices.Users.FindAsync(request.UserIdentifier);
            if (resp == null)
                return NotFoundResult(UserNotFound);

            UserResponse user = resp;
            return new ActionResponse
            {
                PayLoad = user
            };
        }

    }
}
