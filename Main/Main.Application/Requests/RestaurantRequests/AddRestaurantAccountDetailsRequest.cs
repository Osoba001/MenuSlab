using Main.Application.CustomTypes;
using Main.Application.Requests.UserRequests;
using System.Text.Json.Serialization;

namespace Main.Application.Requests.RestaurantRequests
{
    public class AddRestaurantAccountDetailsRequest : Request
    {
        public required Guid Id { get; set; }
        public required BankAccountDetails BankAccountDetails { get; set; }
        [JsonIgnore]
        public UserDevice UserDevice { get; set; }
    }
}
