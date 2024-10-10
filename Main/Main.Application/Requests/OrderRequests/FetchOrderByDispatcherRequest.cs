using Main.Application.CustomTypes;
using Main.Application.Enums;
using System.Diagnostics.Metrics;

namespace Main.Application.Requests.OrderRequests
{
    public class FetchOrderByDispatcherRequest : Request
    {
        public required int Page { get; set; }
        public required int PageSize { get; set; }
        public required Guid DispatcherId { get; set; }
    }
    public class FetchOrderByDispatcherResponse
    {
        public Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public required string UserName { get; set; }
        public required Guid RestaurantId { get; set; }
        public required string RestaurantName { get; set; }
        public required decimal DeleveryFee { get; set; }
        public OrderStatus Status { get; set; }
        public Guid? ReceiverId { get; set; }
        public string? Receiver { get; set; }
        public DateTime CreatedDate { get; set; }
        public required string OrderNumber { get; set; }
        public required decimal DeliveryPaid {  get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal DeliveryCharges { get; set; }
        public decimal DispatcherAmount { get; set; }
        public required Coordinates? DeliveryCoordinate { get; set; }
    }
}
