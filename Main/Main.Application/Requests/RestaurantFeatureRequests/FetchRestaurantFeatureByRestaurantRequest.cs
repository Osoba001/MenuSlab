namespace Main.Application.Requests.RestaurantFeatureRequests
{
    public class FetchRestaurantFeatureByRestaurantRequest:Request
    {
        public required Guid RestaurantId { get; set; }
    }


}
