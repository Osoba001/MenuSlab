using Share;
using Microsoft.AspNetCore.Http;

namespace Share.FileManagement
{
    public interface IFileManagementService
    {
        Task<ActionResponse> DeleteFilesFromCloudAsync(string destination, IEnumerable<string> keys);
        Task<ActionResponse> UploadImageToCloudAsync(IFormFile file, string key, string destination);
        Task<ActionResponse> UploadImagesToCloudAsync(string destination, Dictionary<string, IFormFile> images);
        Task<ActionResponse> UploadVideoToCloudAsync(string destination, IFormFile file, string key,long mbMaxSize=100);
    }

}