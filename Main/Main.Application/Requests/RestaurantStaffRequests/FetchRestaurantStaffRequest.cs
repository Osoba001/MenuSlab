namespace Main.Application.Requests.RestaurantStaffRequests
{
    public class FetchRestaurantStaffRequest : Request
    {
        public required Guid RestaurantId { get; set; }
    }
    public class RestaurantStaffResponse
    {
        public required Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public required string Name { get; set; }
        public required string Role { get; set; }
        public required string StartOfDuty { get; set; }
        public required string EndOfDuty { get; set; }
        public required string StaffId { get; set; }
        public required string PhoneNo { get; set; }
        public required bool IsActive { get; set; }
    }
}
