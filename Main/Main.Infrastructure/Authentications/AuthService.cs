using Main.Application.Requests.UserRequests;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Main.Infrastructure.Authentications
{
    internal class AuthService(MainConfigData optionsConfigData) : IAuthService
    {

        private readonly MainConfigData _configData = optionsConfigData;


        public void GenerateTokens(TokenModel tokenModel)
        {

            List<Claim> claims =
            [
                new Claim(ClaimTypes.NameIdentifier,tokenModel.Id.ToString()),
                new Claim(ClaimTypes.PrimarySid,tokenModel.Session),
                new Claim(ClaimTypes.Role,tokenModel.Role),
                new Claim(ClaimTypes.Name,tokenModel.Name),
            ];

            SecurityTokenDescriptor tokenDescriptor = CreateTokenDescriptor(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            tokenModel.AccessToken = tokenHandler.WriteToken(token);
            tokenModel.RefreshToken = RandomToken();
            tokenModel.RefreshTokenExpireTime = DateTime.UtcNow.Add(_configData.REFRESH_TOKEN_TTL);
        }

        public Guid? RetrieveTokenNameIdentifier(string jwt)
        {
            try
            {
                if (!VerifyExpiredJwtToken(jwt))
                    return null;
                var payload = jwt.Split('.')[1];
                var jsonBytes = ParseBase64WithoutPadding(payload);
                var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
                if (keyValuePairs.TryGetValue("nameid", out var nameIdValue))
                {
                    if (Guid.TryParse(nameIdValue.ToString(), out var value))
                        return value;
                    return null;

                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {
                return null;
            }
        }
        public bool VerifyExpiredJwtToken(string jwtToken)
        {
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configData.AUTH_SECRET_KEY)),
                    ValidateIssuerSigningKey = true,
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out SecurityToken securityToken);

                if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
                    return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private SecurityTokenDescriptor CreateTokenDescriptor(List<Claim> claims)
        {
            var encodedKey = Encoding.UTF8.GetBytes(_configData.AUTH_SECRET_KEY);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(encodedKey), SecurityAlgorithms.HmacSha256),
                Expires = DateTime.UtcNow.Add(_configData.ACCESS_TOKEN_TTL),
            };
            return tokenDescriptor;
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        private static string RandomToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

    }

}
