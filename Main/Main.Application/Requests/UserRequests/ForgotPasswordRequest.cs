using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Main.Application.Requests.UserRequests
{

    public class ForgotPasswordRequest : Request
    {

        [EmailAddress]
        public required string Email { get; set; }
        [JsonIgnore]
        public required UserDevice UserDevice { get; set; }

    }


}
