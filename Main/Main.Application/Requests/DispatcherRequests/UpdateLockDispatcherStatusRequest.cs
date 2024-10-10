namespace Main.Application.Requests.DispatcherRequests
{
    public class UpdateLockDispatcherStatusRequest : Request
    {
        public required Guid Id { get; set; }
        public required bool IsActive { get; set; }
        public required string Message { get; set; }
    }
}
