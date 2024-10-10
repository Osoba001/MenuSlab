using Main.Application.CustomTypes;
using Main.Application.Enums;

namespace Main.Application.Requests.OrderRequests
{
    public class FetchOrderByRestaurantRequest : Request
    {
        public required int Page { get; set; }
        public required int PageSize { get; set; }
        public required Guid RestaurantId { get; set; }

    }

    public class FetchOrderByRestaurantResponse
    {
        public Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public required string UserName { get; set; }
        public required Guid RestaurantId { get; set; }
        public required string RestaurantName { get; set; }
        public required decimal DeleveryFee { get; set; }
        public OrderStatus Status { get; set; }
        public required string OrderType { get; set; }
        public string? TableNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public required string OrderNumber { get; set; }
        public required decimal OrderPaid { get; set; }
        public required decimal RestaurantAmount { get; set; }
        public required decimal OrderCharges { get; set; }
        public required Coordinates? DeliveryCoordinate { get; set; }
    }

}
