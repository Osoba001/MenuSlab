using Main.Application.Requests.CountryRequests;

namespace Main.Application.Requests.PaymentGatewayRequests
{
    public class FetchPaymentGatewayRequest : Request
    {
    }
    public class PaymentGatewayResponse
    {
        public required string Name { get; set; }
        public required IEnumerable<CountryResponse> Countries { get; set; }
    }
    

}
