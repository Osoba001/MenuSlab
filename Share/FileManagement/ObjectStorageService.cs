using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using static Share.ActionResponse;
using Microsoft.AspNetCore.Http;

namespace Share.FileManagement
{
    internal class ObjectStorageService(ObjectStorageConfiguration configuration) : IFileManagementService
    {
        private readonly ObjectStorageConfiguration _storageConfiguration = configuration;
        public async Task<ActionResponse> DeleteFilesFromCloudAsync(string destination, IEnumerable<string> keys)
        {
            try
            {
                Account account = new(
                    _storageConfiguration.StorageName,
                    _storageConfiguration.AccessKey,
                    _storageConfiguration.SecretKey
                );

                Cloudinary cloudinary = new(account);
                List<string> deletedFiles = [];
                List<string> failedFiles = [];

                foreach (var key in keys)
                {
                    var fullKey = $"{_storageConfiguration.AppName}/{destination}/{key}";
                    var deletionParams = new DeletionParams(fullKey);
                    var deletionResult = await cloudinary.DestroyAsync(deletionParams);

                    if (deletionResult.Result == "ok")
                    {
                        deletedFiles.Add(fullKey);
                    }
                    else
                    {
                        failedFiles.Add(fullKey);
                    }
                }

                return new ActionResponse
                {
                    PayLoad = new
                    {
                        deletedFiles,
                        failedFiles,
                        message = "File deletion process completed."
                    }
                };
            }
            catch (Exception ex)
            {
                return ServerExceptionError(ex);
            }
        }
        public async Task<ActionResponse> UploadImagesToCloudAsync(string destination, Dictionary<string, IFormFile> images)
        {
            try
            {
                Account account = new(
                    _storageConfiguration.StorageName,
                    _storageConfiguration.AccessKey,
                    _storageConfiguration.SecretKey
                );

                Cloudinary cloudinary = new(account);
                List<Task<ImageUploadResult>> uploadTasks = [];
                List<string> uploadedUrls = [];
                var uploadResponse= new Dictionary<string, string>();

                foreach (var file in images)
                {
                    var validationResponse = ValidateImage(file.Value);
                    if (!validationResponse.IsSuccess)
                    {
                        uploadResponse.Add(file.Value.Name, $"Failed: {validationResponse.Message}");
                        continue;
                    }
                    uploadResponse.Add(file.Value.Name, "Successful");
                    using var memoryStream = new MemoryStream();
                    await file.Value.CopyToAsync(memoryStream);
                    byte[] imageBytes = memoryStream.ToArray();

                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.Value.Name, new MemoryStream(imageBytes)),
                        PublicId = $"{_storageConfiguration.AppName}/{destination}/{file.Key}"
                    };

                    uploadTasks.Add(cloudinary.UploadAsync(uploadParams));
                }

                var uploadResults = await Task.WhenAll(uploadTasks);

                foreach (var result in uploadResults)
                {
                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        uploadedUrls.Add(result.SecureUrl.ToString());
                    }
                }

                return new ActionResponse
                {
                    PayLoad = new
                    {
                        uploadedUrls,
                        message = uploadResponse
                    }
                };
            }
            catch (Exception ex)
            {
                return ServerExceptionError(ex);
            }
        }
        public async Task<ActionResponse> UploadImageToCloudAsync(IFormFile file, string key, string destination)
        {
           
            try
            {
                var validationResponse = ValidateImage(file);
                if (!validationResponse.IsSuccess)
                    return validationResponse;

                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                byte[] imageBytes = memoryStream.ToArray();

                Account account = new(
                  _storageConfiguration.StorageName,
                  _storageConfiguration.AccessKey,
                  _storageConfiguration.SecretKey
              );

                Cloudinary cloudinary = new(account);
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.Name, new MemoryStream(imageBytes)),
                    PublicId = $"{_storageConfiguration.AppName}/{destination}/{key}",
                };

                var uploadResult = await cloudinary.UploadAsync(uploadParams);

                return new ActionResponse { PayLoad = new { url = uploadResult.SecureUrl.ToString() } };
            }
            catch (Exception ex)
            {

                return ServerExceptionError(ex);
            }
        }

       
        public async Task<ActionResponse> UploadVideoToCloudAsync(string destination, IFormFile file, string key, long maxSize=100)
        {
            try
            {
                var validationResponse = ValidateVideo(file);
                if (!validationResponse.IsSuccess)
                    return validationResponse;

                 long maxVideoSizeInBytes = maxSize * 1024 * 1024; 

                if (file.Length > maxVideoSizeInBytes)
                {
                    return BadRequestResult($"The uploaded video file is too large. Maximum allowed size is {maxSize}MB.");
                }
                Account account = new(
                    _storageConfiguration.StorageName,
                    _storageConfiguration.AccessKey,
                    _storageConfiguration.SecretKey
                );

                Cloudinary cloudinary = new(account);

                var uploadParams = new VideoUploadParams
                {
                    File = new FileDescription(file.Name, file.OpenReadStream()),
                    PublicId = $"{_storageConfiguration.AppName}/{destination}/{key}"
                };

                var uploadResult = await cloudinary.UploadAsync(uploadParams);
                string videoUrl = uploadResult.SecureUrl.ToString();

                return new ActionResponse { PayLoad = new { videoUrl } };
            }
            catch (Exception ex)
            {
                return ServerExceptionError(ex);
            }
        }

        private static ActionResponse ValidateImage(IFormFile file)
        {
            var fileExt = Path.GetExtension(file.FileName)?.TrimStart('.').ToLower();
            if (string.IsNullOrWhiteSpace(fileExt) || !IsValidImageExtension(fileExt))
                return BadRequestResult("Invalid image type.");
            return SuccessResult();
        }

        private static ActionResponse ValidateVideo(IFormFile file)
        {
            var fileExt = Path.GetExtension(file.FileName)?.TrimStart('.').ToLower();
            if (string.IsNullOrWhiteSpace(fileExt) || !IsValidVideoExtension(fileExt))
                return BadRequestResult("Invalid video type.");
            return SuccessResult();
        }

        private static readonly HashSet<string> ValidImageExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        "png", "jpeg", "webp", "bmp", "pbm", "tga", "gif", "tiff", "psd", "raw", "heif", "indd", "jfi", "jfif", "jpe", "jif", "jpg"
    };

        private static readonly HashSet<string> ValidVideoExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        "mp4", "avi", "mov", "mkv", "flv", "wmv", "webm", "m4v"
    };

        private static bool IsValidImageExtension(string extension)
        {
            return ValidImageExtensions.Contains(extension);
        }

        private static bool IsValidVideoExtension(string extension)
        {
            return ValidVideoExtensions.Contains(extension);
        }
    }
}
