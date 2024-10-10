using Main.Application.CustomTypes;

namespace Main.Application.Requests.DispatcherRequests
{
    public class GetDispatcherFullDetailRequest : Request
    {
        public Guid Id { get; set; }
    }
    public class GetDispatcherFullDetailResponse
    {
        public required Guid Id { get; set; }
        public required string Number { get; set; }
        public required string Name { get; set; }
        public required string PhoneNo { get; set; }
        public required string Rider { get; set; }
        public required string Currency { get; set; }
        public string ActiveMessage { get; set; }=string.Empty;
        public required bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public required decimal Charge { get; set; }
        public required decimal NetBalance { get; set; }
        public required decimal Balance { get; set; }
        public BankAccountDetails? BankAccount { get; set; }
        public required string Country { get; set; }
    }
}
