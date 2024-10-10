using Newtonsoft.Json;

namespace Main.Application.Requests.UserRequests
{
    public class ChangePasswordRequest : Request
    {
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }
        [JsonIgnore]
        public required UserDevice UserDevice { get; set; }
    }
}