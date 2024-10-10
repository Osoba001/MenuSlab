using Main.Application.Requests.UserRequests;
using System.Text.Json.Serialization;

namespace Main.Application.Requests.RestaurantRequests
{
    public class DeleteRestaurantRequest : Request
    {
        public required Guid Id { get; set; }
        public required string Password { get; set; }
        [JsonIgnore]
        public UserDevice UserDevice { get; set; }
    }
    
    
}
