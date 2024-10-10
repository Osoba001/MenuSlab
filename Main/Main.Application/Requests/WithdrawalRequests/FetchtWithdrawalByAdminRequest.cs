using Main.Application.Enums;

namespace Main.Application.Requests.WithdrawalRequests
{
    public class FetchtWithdrawalByAdminRequest : Request
    {
        public required string Country { get; set; }
        public required string SubNumber { get; set; }
        public required WithdrawalSourceType SourceType { get; set; }
        public required int PageSize { get; set; }
        public required int Page { get; set; }
    }

    public class WithdrawalByAdminResponse
    {
        public Guid Id { get; set; }
        public required decimal Amount { get; set; }
        public required decimal Charges { get; set; }
        public required string WithdrawStatus { get; set; }
        public required string Message { get; set; } 
        public DateTime RequestedDate { get; set; }
        public DateTime? RespondedDate { get; set; }
        public Guid? SystemAdminId { get; set; }
        public string? SystemAdmin { get; set; }
        public required decimal NetBalance { get; set; }
        public required string Number { get; set; }
        public required string SourceType { get; set; }
        public string? StaffNumber { get; set; }

    }
}
