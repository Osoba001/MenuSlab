using Main.Application.Requests.UserRequests;
using Newtonsoft.Json;

namespace Main.Application.Requests.OrderRequests
{
    public class AddPaymentToOrderRequest:Request
    {
        public required Guid OrderId { get; set; }
        public required decimal AmountForOrder { get; set; }
        public required decimal AmountForDelivery { get; set; }
        public required OrderPaymentType OrderPaymentType { get; set; }
        [JsonIgnore]
        public UserDevice UserDevice { get; set; }
    }
    public enum OrderPaymentType
    {
        OrderOnly,
        DeliveryOnly,
        OrderAndDelivery
    }
}
