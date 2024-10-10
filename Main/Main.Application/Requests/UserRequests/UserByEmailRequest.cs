namespace Main.Application.Requests.UserRequests
{
    public class UserByEmailRequest : Request
    {
        public required string Email { get; set; }
    }
}
