namespace Main.Application.Requests.DispatcherRequests
{
    public class DeleteDispatcherRequest : Request
    {
        public required Guid Id { get; set; }
        public required string Password { get; set; }
    }
}
