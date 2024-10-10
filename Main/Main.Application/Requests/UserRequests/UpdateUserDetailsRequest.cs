namespace Main.Application.Requests.UserRequests
{
    public class UpdateUserDetailsRequest : Request
    {
        public required string Name { get; set; }
        public required string Phone { get; set; }

    }
}
