namespace Main.Application.Requests.RestaurantStaffRequests
{
    public class AddRestaurantStaffRequest : Request
    {
        public required Guid RestaurantId { get; set; }
        public required IEnumerable<RestaurantStaffDto> Staffs { get; set; }
    }
    public class RestaurantStaffDto
    {
        public required Guid UserId { get; set; }
        public required string Role { get; set; }
        public required string StartOfDuty { get; set; }
        public required string EndOfDuty { get; set; }
        public string StaffId { get; set; }=string.Empty;
    }
}
