using Main.Application.Enums;

namespace Main.Application.Requests.FundTransactionRequests
{
    public class FundAccountRequest:Request
    {
        public required decimal Amount { get; set; }
        public Guid Id { get; set; }
        public required string TransactionId { get; set; }
        public required string Currency { get; set; }
        public required FundTransactionType FundTransactionType { get; set; }
    }
    
}
