using Main.Application.Enums;

namespace Main.Application.Requests.WithdrawalRequests
{
    public class FetchRestaurantWithdrawalsRequest : Request
    {
        public required int Page { get; set; }
        public required int PageSize { get; set; }
        public required Guid RestaurantId { get; set; }
    }

    
}
