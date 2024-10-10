namespace Main.Application.Requests.RestaurantFeatureRequests
{
    public class RemoveFeatureFromRestaurantRequest:Request
    {
        public required Guid RestaurantId { get; set; }
        public required List<Guid> FeatureId { get; set; }
    }

}
