namespace Main.Application.Requests.PaymentGatewayRequests
{
    public class AddBankRequest:Request
    {
        public required string BankName { get; set; }
        public required string BankCode { get; set; }
        public required string CountryId { get; set; }
    }

    

}
