namespace Main.Application.Requests.OfferOrderRequests
{
    public class CreateOfferOrderRequest:Request
    {
        public Guid? ReceiverId { get; set; } = null;
        public required string PhoneNo { get; set; }
    }

    
}
