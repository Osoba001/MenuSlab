namespace Main.Application.Requests.RestaurantRequests
{
    public class FetchRestaurantsByAdminRequest : Request
    {
        public required string Country { get; set; }
        public required int Page { get; set; }
        public required int PageSize { get; set; }
    }
}
