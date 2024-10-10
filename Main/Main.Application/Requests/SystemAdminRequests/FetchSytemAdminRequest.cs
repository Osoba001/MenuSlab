using Main.Application.Enums;

namespace Main.Application.Requests.SystemAdminRequests
{
    public class FetchSytemAdminRequest:Request
    {
        public required string CountryId { get; set; }
    }
    public class FetchSytemAdminResponse
    {
        public required Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public required string Name { get; set; }
        public required string Country { get; set; }
        public required string StaffNumber { get; set; }
        public StaffRole Role { get; set; }
        public required bool IsActive { get; set; }
        public DateTime JoinDate { get; set; }
    }
}
