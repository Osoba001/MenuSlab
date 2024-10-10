using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Main.Application.Requests.UserRequests
{
    public class CreateUserRequest : Request
    {

        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Name { get; set; }
        public required string PhoneNo { get; set; }
        public required string Country { get; set; }
        [JsonIgnore]
        public required UserDevice UserDevice { get; set; }
    }
}
