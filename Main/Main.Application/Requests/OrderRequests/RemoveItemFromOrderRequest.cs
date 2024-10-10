namespace Main.Application.Requests.OrderRequests
{
    public class RemoveItemFromOrderRequest : Request
    {
        public required Guid OrderId { get; set; }
        public required Dictionary<Guid, int> OrderedItems { get; set; }
    }
}
