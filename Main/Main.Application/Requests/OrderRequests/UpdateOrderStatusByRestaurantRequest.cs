using Main.Application.Enums;

namespace Main.Application.Requests.OrderRequests
{
    public class UpdateOrderStatusByRestaurantRequest : Request
    {
        public required Guid Id { get; set; }
        public Guid RestaurantStaffId { get; set; }
        public string DeclineMessage { get; set; } = string.Empty;
        public int? WaitTimeInMinutes { get; set; }
        public OrderStatus Status { get; set; }
    }

}
