
using Main.Application.CustomTypes;
using Main.Application.Requests.UserRequests;
using System.Text.Json.Serialization;

namespace Main.Application.Requests.DispatcherRequests
{
    public class AddDispatcherAccountDetailsRequest : Request
    {
        public required string Bank { get; set; }
        public required string AccountNo { get; set; }
        public required string AccountName { get; set; }

        [JsonIgnore]
        public UserDevice UserDevice { get; set; }
    }
}
