using Main.Application.Enums;
using Main.Application.Requests.UserRequests;
using Main.Infrastructure.Authentications;
using Main.Infrastructure.Database;
using Share.FileManagement;



namespace Main.Infrastructure.RequestHandlers.UserHandlers
{
    internal class UploadProfilePhotoHandler(IFileManagementService fileService) : IRequestHandler<UploadProfilePhotoRequest>
    {
        private readonly IFileManagementService _fileService = fileService;

        public async Task<ActionResponse> HandleAsync(UploadProfilePhotoRequest request, CancellationToken cancellationToken = default)
        {
            return await _fileService.UploadImageToCloudAsync(request.File, request.UserIdentifier.ToString(), FileDestination.UserProfile.ToString());

        }
    }
}
