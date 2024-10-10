namespace Main.Application.Requests.WithdrawalRequests
{
    public class ApproveWithdrawFundRequest : Request
    {
        public Guid Id { get; set; }
        public Guid AdminId { get; set; }
        public required string Message { get; set; }
        public required bool Approve { get; set; }
    }
}
