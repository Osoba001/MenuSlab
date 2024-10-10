using Main.Application.CustomTypes;

namespace Main.Application.Requests.RestaurantRequests
{
    public class CreateRestaurantRequest:Request
    {
        public required string Name { get; set; }
        public required string PhoneNo { get; set; }
        public required string Description { get; set; }
        public required string Currency { get; set; }
        public required Coordinates Coordinates { get; set; }
        public required string StreetAddress { get; set; }
        public required string Country { get; set; }
        public bool HomeDelivery { get; set; }
        public bool OnPremise { get; set; }
        public string OpenTime { get; set; } = string.Empty;
        public string CloseTime { get; set; } = string.Empty;
    }
    
    
}
