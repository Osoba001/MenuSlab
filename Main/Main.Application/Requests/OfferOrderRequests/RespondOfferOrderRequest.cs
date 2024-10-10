using Main.Application.CustomTypes;
using Main.Application.Enums;

namespace Main.Application.Requests.OfferOrderRequests
{
    public class RespondOfferOrderRequest : Request
    {
        public Guid? Id { get; set; }=null;
        public string? Code { get; set; }=null;
        public Coordinates? DeliveryCoordinates { get; set; } = null;
        public bool IsAccepted { get; set; }
    }

    public class DeleteOfferOrderRequest : Request
    {
        public Guid Id { get; set; }
    }


}
