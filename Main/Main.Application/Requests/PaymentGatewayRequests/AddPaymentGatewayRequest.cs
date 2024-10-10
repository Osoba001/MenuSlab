namespace Main.Application.Requests.PaymentGatewayRequests
{
    public class AddPaymentGatewayRequest:Request
    {
        public required string Name { get; set; }
        public required string ImplementationName { get; set; }
    } 

    

}
