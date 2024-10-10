namespace Main.Application.Requests.RestaurantRequests
{
    public class VerifyRestaurantRequest : Request
    {
        public required Dictionary<Guid, string> IdNumbers { get; set; }
    }
    
    
}
