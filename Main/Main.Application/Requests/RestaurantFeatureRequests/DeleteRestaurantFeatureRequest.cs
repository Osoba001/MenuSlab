namespace Main.Application.Requests.RestaurantFeatureRequests
{
    public class DeleteRestaurantFeatureRequest:Request
    {
        public required Guid Id { get; set; }
    }



}
