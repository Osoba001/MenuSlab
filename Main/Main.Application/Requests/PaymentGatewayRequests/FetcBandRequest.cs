namespace Main.Application.Requests.PaymentGatewayRequests
{
    public class FetchBankRequest : Request
    {
        public required string Country { get; set; }
    }
    public class BankResponse
    {
        public required string BankName { get; set; }
        public required string BankCode { get; set; }
    }
}
