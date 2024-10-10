namespace Main.Application.Requests.CountryRequests
{
    public class UpdateCountryPaymentdetailRequest:Request
    {
        
        public required Guid PaymentGatewayId { get; set; }
        public required IEnumerable<UpdateCountryPaymentDto> PaymentDetails { get; set; }
    }

    public class UpdateCountryPaymentDto
    {
        public required string Country { get; set; }
        public required decimal MinWithDrawal { get; set; }
        public required decimal MaxWithDrawal { get; set; }
        public required decimal MaxTransfer { get; set; }
        public required decimal MinTransfer { get; set; }
        public required decimal BaseOrderChange { get; set; }
    }
}
