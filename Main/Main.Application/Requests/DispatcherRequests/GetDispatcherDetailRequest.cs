namespace Main.Application.Requests.DispatcherRequests
{
    public class GetDispatcherDetailRequest : Request
    {
        public required Guid Id { get; set; }
    }
    public class GetDispatcherDetailResponse
    {
        public required Guid Id { get; set; }
        public required string Number { get; set; }
        public required string Name { get; set; }
        public required string PhoneNo { get; set; }
        public required string Country { get; set; }
        public required string Currency { get; set; }
        public required string Rider { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
