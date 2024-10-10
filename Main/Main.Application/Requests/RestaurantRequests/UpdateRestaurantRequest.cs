using Main.Application.CustomTypes;
using Main.Application.Requests.UserRequests;
using System.Text.Json.Serialization;

namespace Main.Application.Requests.RestaurantRequests
{
    public class UpdateRestaurantRequest : Request
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string PhoneNo { get; set; }
        public required string Description { get; set; }
        public required Coordinates Coordinates { get; set; }
        public required string StreetAddress { get; set; }
        public double RestaurantRadius { get; set; }
        public required string DeliveryCondiction { get; set; }
        public required bool HomeDelivery { get; set; }
        public required bool OnPremise { get; set; }
        public required bool PayWithApp { get; set; }
        public string OpenTime { get; set; }=string.Empty;
        public string CloseTime { get; set; }= string.Empty;
        public required bool IsActive { get; set; }

        [JsonIgnore]
        public UserDevice UserDevice { get; set; }
    }

}
