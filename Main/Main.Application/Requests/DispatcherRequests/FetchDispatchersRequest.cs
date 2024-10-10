namespace Main.Application.Requests.DispatcherRequests
{
    public class FetchDispatchersRequest : Request
    {
        public required int Page { get; set; }
        public required int PageSize { get; set; }
        public required string SubNumber { get; set; }
        public required string Country { get; set; }
    }

    public class FetchDispatcherResponse
    {
        public required Guid Id { get; set; }
        public required string Number { get; set; }
        public required string Name { get; set; }
        public required string PhoneNo { get; set; }
        public required string Rider { get; set; }
        public required string Country { get; set; }
        public required bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

}
