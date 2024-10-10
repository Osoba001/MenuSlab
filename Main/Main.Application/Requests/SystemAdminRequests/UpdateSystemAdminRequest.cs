using Main.Application.Enums;

namespace Main.Application.Requests.SystemAdminRequests
{
    public class UpdateSystemAdminRequest : Request
    {
        public required Guid Id { get; set; }
        public required string StaffNumber { get; set; }
        public StaffRole Role { get; set; }
        public required bool IsActive { get; set; }
    }
    
}
