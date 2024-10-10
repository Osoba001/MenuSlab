using Main.Application.CustomTypes;
using Main.Application.Requests.RestaurantStaffRequests;

namespace Main.Application.Requests.RestaurantRequests
{
    public class GetRestaurantFullDetailRequest : Request
    {
        public required Guid Id { get; set; }
    }
    public class GetRestaurantFullDetailResponse
    {
        public required Guid Id { get; set; }
        public required string Number { get; set; }
        public required string PhoneNo { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Currency { get; set; }
        public required Coordinates Coordinates { get; set; }
        public required string StreetAddress { get; set; }
        public required string Country { get; set; }
        public double RestaurantRadius { get; set; }
        public required bool HomeDelivery { get; set; }
        public required bool OnPremise { get; set; }
        public required bool PayWithApp { get; set; }
        public required string DeliveryCondiction { get; set; }
        public bool EnableOpenMenuCode { get; set; }
        public string? OpenTime { get; set; }
        public string? CloseTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public BankAccountDetails? BankAccount { get; set; }
        public required string LockedMessage { get; set; }
        public required bool IsLocked { get; set; }
        public required bool IsActive { get; set; }
        public required bool IsCreator { get; set; }
        public decimal NetBalance { get; set; }
        public decimal Charges { get; set; }
        public string? OpenCode { get; set; } = null;
        public required IEnumerable<RestaurantStaffResponse> Staff { get; set; }
    }
}
