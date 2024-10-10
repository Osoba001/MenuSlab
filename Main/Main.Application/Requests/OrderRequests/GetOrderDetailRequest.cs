using Main.Application.CustomTypes;
using Main.Application.Enums;
using System.Diagnostics.Metrics;

namespace Main.Application.Requests.OrderRequests
{
    public class GetOrderDetailRequest : Request
    {
        public required Guid Id { get; set; }
    }

    public class GetOrderDetailsResponse
    {
        public Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public required string UserName { get; set; }
        public required Guid RestaurantId { get; set; }
        public required string RestaurantName { get; set; }
        public required List<OrderedItemResponse> OrderedItems { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime? WaitTimeInMinutes { get; set; }
        public string? TableNumber { get; set; }
        public decimal RestaurantAmount { get; set; }
        public decimal OrderCharges { get; set; }
        public decimal DeliveryFee { get; set; }
        public required string OrderType { get; set; }
        public required string Currency { get; set; }
        public Coordinates? DeliveryCoordinates { get; set; }
        public Guid? ReceiverId { get; set; }
        public string? ReceiverName { get; set; }
        public string CustomerMessage { get; set; } = string.Empty;
        public string DeclineMessage { get; set; } = string.Empty;
        public Guid? DispatcherId { get; set; }
        public string? DispatcherName { get; set; }
        public Guid? RestaurantStaffId { get; set; }
        public string? RestaurantStaffName { get; set; }
        public DateTime CreatedDate { get; set; }
        public required string OrderNumber { get; set; }
        public required decimal OrderPaid { get; set; }
        public required decimal DeleveryPaid { get; set; }
    }
    public class OrderedItemResponse
    {
        public required string ItemName { get; set; }
        public required int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount => UnitPrice * Quantity;
    }
}
