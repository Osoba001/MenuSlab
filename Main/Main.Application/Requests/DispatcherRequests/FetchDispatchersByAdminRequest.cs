namespace Main.Application.Requests.DispatcherRequests
{
    public class FetchDispatchersByAdminRequest : Request
    {
        public required string Country { get; set; }
        public required int Page { get; set; }
        public required int PageSize { get; set; }
    }
    public class FetchFullDispatcherResponse
    {
        public required Guid Id { get; set; }
        public required string Number { get; set; }
        public required string Name { get; set; }
        public required string PhoneNo { get; set; }
        public required string Rider { get; set; }
        public required string Country { get; set; }
        public required decimal Charge { get; set; }
        public required decimal Balance { get; set; }
        public required decimal NetBalance { get; set; }
        public required bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
