using Main.Application.Enums;
using Main.Application.Requests.UserRequests;
using System.Text.Json.Serialization;

namespace Main.Application.Requests.WithdrawalRequests
{
    public class WithdrawFundRequest : Request
    {
        public required Guid DispatcherId { get; set; }
        public required decimal Amount { get; set; }

        public required WithdrawalSourceType SourceType { get; set; }
    }
}
