using Main.Application.CustomTypes;
using Main.Application.Enums;

namespace Main.Application.Requests.OfferOrderRequests
{
    public class FetchOfferOrderRequest:Request
    {
    }

    public class FetchOfferOrderResponse
    {
        public Guid Id { get; set; }
        public required Guid SenderId { get; set; }
        public required string Sender { get; set; }
        public required string Status { get; set; }
        public required string Code { get; set; }
        public Guid? ReceiverId { get; set; } = null;
        public string? Receiver { get; set; } = null;
        public Coordinates? DeliveryCoordinates { get; set; } = null;
        public DateTime CreatedDate { get; set; }
    }
}
