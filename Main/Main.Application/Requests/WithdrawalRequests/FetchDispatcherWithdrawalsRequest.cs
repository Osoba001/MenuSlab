using Main.Application.CustomTypes;
using Main.Application.Enums;
using System.Diagnostics.Metrics;

namespace Main.Application.Requests.WithdrawalRequests
{
    public class FetchDispatcherWithdrawalsRequest : Request
    {
        public required int Page { get; set; }
        public required int PageSize { get; set; }
        public required Guid DispatchertId { get; set; }
    }
    public class WithdrawalDispatcherResponse
    {
        public Guid Id { get; set; }
        public required decimal Amount { get; set; }
        public required decimal Charges { get; set; }
        public required string WithdrawStatus { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime RequestedDate { get; set; }
        public DateTime? RespondedDate { get; set; }
        public required decimal NetBalance { get; set; }
        public required string CountryId { get; set; }

    }
}
