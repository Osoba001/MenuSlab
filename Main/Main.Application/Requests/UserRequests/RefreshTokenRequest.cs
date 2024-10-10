namespace Main.Application.Requests.UserRequests
{
    public class RefreshTokenRequest : Request
    {
        public required string RefreshToken { get; set; }
        public required string AccessToken { get; set; }
    }
}
