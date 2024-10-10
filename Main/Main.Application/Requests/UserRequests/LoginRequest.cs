using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace Main.Application.Requests.UserRequests
{
    public class LoginRequest : Request
    {
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
        [JsonIgnore]
        public required UserDevice UserDevice { get; set; } 

    }
    public class TokenModel
    {
        public Guid Id { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public required string Email { get; set; }
        public required string Name { get; set; }
        public required string Role { get; set; }
        [JsonIgnore]
        public DateTime RefreshTokenExpireTime { get; set; }
        [JsonIgnore]
        public required string Session { get; set; }
    }
    public class UserDevice
    {
        public required string DeviceType { get; set; }
        public required string Brand { get; set; }
        public required string Model { get; set; }

        public override string ToString()
        {
            return $"Device Type: {DeviceType}, Brand: {Brand}, Model: {Model}";
        }
    }
}
