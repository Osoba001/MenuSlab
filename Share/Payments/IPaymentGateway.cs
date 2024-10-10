namespace Share.Payments
{
    public interface IPaymentGateway
    {
        Task<ActionResponse> ProcessWithdrawal(WithdrawDto paymentDetail);
        Task<bool> VerifyWithdrawal(string transactionId, decimal amount);
        Task<bool> VerifyReceivedFund(string transactionId, decimal amount);
    }
    public class WithdrawDto
    {
        public required string BankCode { get; set; }
        public required string AccountNo { get; set; }
        public required string AccountName { get; set; }
        public required decimal Amount { get; set; }
        public required string Narration { get; set; }
        public required string Currency { get; set; }
        public required string ReferenceNo { get; set; }
    }
}
