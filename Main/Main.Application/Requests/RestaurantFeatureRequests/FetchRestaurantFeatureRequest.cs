namespace Main.Application.Requests.RestaurantFeatureRequests
{
    public class FetchRestaurantFeatureRequest:Request
    {
        
    }

    public class RestaurantFeatureResponse
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
    }

}
