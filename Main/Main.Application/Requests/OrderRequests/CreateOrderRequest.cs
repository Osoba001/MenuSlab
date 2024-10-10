using Main.Application.CustomTypes;
using Main.Application.Enums;
using System;

namespace Main.Application.Requests.OrderRequests
{
    public class CreateOrderRequest : Request
    {
        public required Guid RestaurantId { get; set; }
        public required OrderType OrderType { get; set; }
        public required Dictionary<Guid,int> OrderedItems { get; set; }
        public string? TableNumber { get; set; }
        public Coordinates? DeliveryCoordinates { get; set; } = null;
        public Guid? ReceiverId { get; set; } = null;
    }


    
    
}
