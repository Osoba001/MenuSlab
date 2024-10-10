namespace Main.Application.Requests.CountryRequests
{
    public class FetchCountriesFullDetailRequest : Request
    {
        
    }
    public class CountryFullDetailResponse
    {
        public required string Name { get; set; }
        public required string Currency { get; set; }
        public decimal MinWithDrawal { get; set; }
        public decimal MaxWithDrawal { get; set; }
        public decimal MaxTransfer { get; set; }
        public decimal MinTransfer { get; set; }
        public decimal BaseOrderChange { get; set; }
        public Guid? PaymentGatewayId { get; set; }
        public string? PaymentGateway { get; set; }
    }
}
