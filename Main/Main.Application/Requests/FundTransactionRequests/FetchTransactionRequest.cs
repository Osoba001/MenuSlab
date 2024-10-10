namespace Main.Application.Requests.FundTransactionRequests
{
    public class FetchTransactionRequest:Request
    {
        public required int Page { get; set; }
        public required int PageSize { get; set; }
    }
   
    public class TransactionResponse
    {
        public required Guid Id { get; set; }
        public required decimal Amount { get; set; }
        public required string TransactionId { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public required string FundTransactionType { get; set; }
        public Guid? ReceiverId { get; set; }
        public string? Receiver { get; set; }
        public Guid? SenderId { get; set; }
        public string? Sender { get; set; }
        public Guid? OrderId { get; set; }
        public string? Restaurant { get; set; }
        public bool IsCredit { get; set; }
    }
   
}
