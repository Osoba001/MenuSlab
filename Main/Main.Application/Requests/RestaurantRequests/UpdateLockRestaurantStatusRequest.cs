namespace Main.Application.Requests.RestaurantRequests
{
    public class UpdateLockRestaurantStatusRequest : Request
    {
        public required Guid Id { get; set; }
        public required bool Status{ get; set; }
        public required string Message { get; set; }
    }

}
