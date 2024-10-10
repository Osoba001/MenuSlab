

using CloudinaryDotNet;

namespace Share.Payments.PaymentGateways
{
    internal class Flutterwave : IPaymentGateway
    {
        public Task<ActionResponse> ProcessWithdrawal(WithdrawDto paymentDetail)
        {
            throw new NotImplementedException();
        }

        public Task<bool> VerifyReceivedFund(string transactionId, decimal amount)
        {
            throw new NotImplementedException();
        }

        public Task<bool> VerifyWithdrawal(string transactionId, decimal amount)
        {
            throw new NotImplementedException();
        }
    }

    public class FlutterwavePayoutResponse
    {
        public required string Status { get; set; }
        public required string Message { get; set; }
        public  PayoutData Data { get; set; }
    }

    public class PayoutData
    {
        public required string Id { get; set; }
        public required string AccountNumber { get; set; }
        public required string BankName { get; set; }
        public required decimal Amount { get; set; }
        public  string Currency { get; set; }
        public string Status { get; set; }
        public string Reference { get; set; }
    }
}
