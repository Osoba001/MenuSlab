using Main.Application.Enums;

namespace Main.Application.Requests.OrderRequests
{
    public class FetchOrderByUserRequest : Request
    {

    }

    public class FetchOrderResponse
    {
        public Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public required string UserName { get; set; }
        public required Guid RestaurantId { get; set; }
        public required string RestaurantName { get; set; }
        public string? TableNumber { get; set; }
        public required decimal Amount { get; set; }
        public required decimal Charges { get; set; }
        public OrderStatus Status { get; set; }
        public required string OrderType { get; set; }
        public Guid? ReceiverId { get; set; }
        public string? Receiver { get; set; }
        public DateTime CreatedDate { get; set; }
        public required string OrderNumber { get; set; }
    }

}
