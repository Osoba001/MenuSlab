namespace Main.Application.Requests.RestaurantRequests
{
    public class FetchRestaurantByLocationRequest:Request
    {
        public required decimal Latitude { get; set; }
        public required decimal Longitude { get; set; }
    }
    public class FetchRestaurantByLocationResponse
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string StreetAddress { get; set; }
        public required string Number{ get; set; }
        public required decimal Latitude { get; set; }
        public required decimal Longitude { get; set; }
        public required double RestaurantRadius { get; set; }
        public required string OpenCode { get; set; }
        public required string OpenTime { get; set; } 
        public required string CloseTime { get; set; }
        public required IEnumerable<int> Rates { get; set; }
    }
}
