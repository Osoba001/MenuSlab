namespace Main.Application.Requests.RestaurantRequests
{
    public class FetchRestaurantsRequest : Request
    {
        public required int Page { get; set; }
        public required int PageSize { get; set; }
        public required string SubNumber { get; set; }
        public required string Country { get; set; }
    }
    public class FetchRestaurantResponse
    {
        public required Guid Id { get; set; }
        public required string Number { get; set; }
        public required string Name { get; set; }
        public required bool HomeDelivery { get; set; }
        public required bool OnPremise { get; set; }
        public required bool PayWithApp { get; set; }
        public required string Coutry { get; set; }
        public required string StreetAddress { get; set; }
        public required IEnumerable<int> Rates { get; set; }

    }
}
