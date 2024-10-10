namespace Main.Application.Requests.OrderRequests
{
    public class AddItemToOrderRequest:Request
    {
        public Guid OrderId { get; set; }
        public required Dictionary<Guid, int> OrderedItems { get; set; }
    }
}
