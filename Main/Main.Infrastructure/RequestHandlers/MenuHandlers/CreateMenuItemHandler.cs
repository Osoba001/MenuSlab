using Main.Application.Enums;
using Main.Application.Requests.MenuRequests;
using Main.Infrastructure.Database;
using Main.Infrastructure.Database.Entities;
using Microsoft.AspNetCore.Http;
using Share.FileManagement;

namespace Main.Infrastructure.RequestHandlers.MenuHandlers
{
    internal class CreateMenuItemHandler(MenuslabDbContext dbContext, IFileManagementService fileService) : IRequestHandler<CreateMenuItemRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;
        private readonly IFileManagementService _fileService = fileService;

        public async Task<ActionResponse> HandleAsync(CreateMenuItemRequest request, CancellationToken cancellationToken = default)
        {
            Dictionary<string, IFormFile> images = [];

            var menuItems= new List<MenuItem>();
            foreach (var item in request.Items)
            {
                var newItem = new MenuItem { CategoryId = request.CategoryId, Name = item.Name, Description = item.Description, PackagePrice = item.PackagePrice, Price = item.Price, Index = item.Index, IsAvailable = true, Discount = item.Discount };
                menuItems.Add(newItem);
                images.Add(newItem.Id.ToString(), item.Photo);
            }

            _= _fileService.UploadImagesToCloudAsync(FileDestination.MenuItem.ToString(), images);
            _dbContext.MenuItems.AddRange(menuItems);
            return await _dbContext.CompleteAsync(menuItems.Select(x=> new {x.Name, x.Id}));
        }
    }
}
