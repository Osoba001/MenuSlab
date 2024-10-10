using Main.Application.Requests.UserRequests;

namespace Main.Infrastructure.Authentications
{
    public interface IAuthService
    {
        void GenerateTokens(TokenModel tokenModel);
        bool VerifyExpiredJwtToken(string jwtToken);
        Guid? RetrieveTokenNameIdentifier(string jwt);
    }
}