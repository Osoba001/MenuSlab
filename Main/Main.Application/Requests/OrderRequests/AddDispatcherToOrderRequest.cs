namespace Main.Application.Requests.OrderRequests
{
    public class AddDispatcherToOrderRequest : Request
    {
        public required Guid Id { get; set; }
        public required Guid DispatcherId { get; set; }
        public required decimal Amount { get; set; }
    }
}
