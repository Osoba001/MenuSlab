using Main.Application.Enums;

namespace Main.Application.Requests.SystemAdminRequests
{
    public class CreateSystemAdminRequest:Request
    {
        public required string CountryId { get; set; }
        public required Guid UserId { get; set; }
        public required string StaffNumber { get; set; }
        public StaffRole Role { get; set; }
    }
}
