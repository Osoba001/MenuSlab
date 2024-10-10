namespace Main.Application.Requests.RestaurantStaffRequests
{
    public class RemoveRestaurantStaffRequest : Request
    {
        public required Guid RestaurantId { get; set; }
        public required List<Guid> Ids { get; set; }
    }

}
