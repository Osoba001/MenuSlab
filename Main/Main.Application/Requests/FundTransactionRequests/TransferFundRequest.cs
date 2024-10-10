using Main.Application.Requests.UserRequests;

namespace Main.Application.Requests.FundTransactionRequests
{
    public class TransferFundRequest:Request
    {
        public required Guid ReceiverId { get; set; }
        public required decimal Amound { get; set; }
        public UserDevice UserDevice { get; set; }
    }
}
