namespace Main.Application.Requests.RestaurantFeatureRequests
{
    public class UpsertRestaurantFeatureRequest:Request
    {
        public required List<RestaurantFeatureDto> RestaurantFeatures { get; set; }
    }
    public class RestaurantFeatureDto
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
    }

}
