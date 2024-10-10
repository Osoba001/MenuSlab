namespace Main.Application.Requests.RestaurantStaffRequests
{
    public class UpdateRestaurantStaffRequest : Request
    {
        public required Guid Id { get; set; }
        public required Guid RestaurantId { get; set; }
        public required string Role { get; set; }
        public required string StartOfDuty { get; set; }
        public required string EndOfDuty { get; set; }
        public string StaffId { get; set; } = string.Empty;
        public required bool IsActive { get; set; }
    }

}
