using Microsoft.AspNetCore.Http;

namespace Main.Application.Requests.UserRequests
{
    public class UploadProfilePhotoRequest : Request
    {
        public required IFormFile File { get; set; }
    }
    

}
