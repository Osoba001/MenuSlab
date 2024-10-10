namespace Main.Application.Requests.OrderRequests
{
    public class CompleteOrderRequest : Request
    {
        public required Guid Id { get; set; }
    }

}
