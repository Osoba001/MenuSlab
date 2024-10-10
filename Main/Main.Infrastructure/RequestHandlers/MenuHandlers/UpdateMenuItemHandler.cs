using Main.Application.Enums;
using Main.Application.Requests.MenuRequests;
using Main.Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Share.FileManagement;

namespace Main.Infrastructure.RequestHandlers.MenuHandlers
{
    internal class UpdateMenuItemHandler(MenuslabDbContext dbContext, IFileManagementService fileService) : IRequestHandler<UpdateMenuItemRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;
        private readonly IFileManagementService _fileService = fileService;

        public async Task<ActionResponse> HandleAsync(UpdateMenuItemRequest request, CancellationToken cancellationToken = default)
        {
            var isOwner = await _dbContext.Restaurants
             .AnyAsync(x => x.Id == request.RestaurantId && x.UserId == request.UserIdentifier);
            if (!isOwner) return Forbiden("This is not your restaurant");

            Dictionary<string, IFormFile> images = request.MenuItems.Where(x => x.Photo != null).Select(x => new { Id = x.Id.ToString(), x.Photo }).ToDictionary(x => x.Id, x => x.Photo)!;
            _= _fileService.UploadImagesToCloudAsync(FileDestination.MenuItem.ToString(), images);
            var existingItems = await _dbContext.MenuItems.Where(x =>x.Category.RestaurantId==request.RestaurantId && request.MenuItems.Select(itm => itm.Id).Contains(x.Id))
                .ToDictionaryAsync(x => x.Id, x => x);
           
            foreach(var item in request.MenuItems)
            {
                if(existingItems.TryGetValue(item.Id, out var menuItem))
                {
                    var change = new { menuItem.Name, menuItem.DiscountCondiction, menuItem.Discount, menuItem.IsAvailable, menuItem.Description, menuItem.PackagePrice, menuItem.Price, menuItem.Index };
                    menuItem.PackagePrice=item.PackagePrice;
                    menuItem.Price=item.Price;
                    menuItem.Index=item.Index;
                    menuItem.Description=item.Description;
                    menuItem.IsAvailable=item.IsAvailable;
                    menuItem.Discount=item.Discount;
                    menuItem.DiscountCondiction=item.DiscountCondiction;

                    _dbContext.Entry(menuItem).Property(x=>x.PackagePrice).IsModified=change.PackagePrice!=item.PackagePrice;
                    _dbContext.Entry(menuItem).Property(x=>x.Price).IsModified=change.Price!=item.Price;
                    _dbContext.Entry(menuItem).Property(x=>x.Index).IsModified=change.Index != item.Index;
                    _dbContext.Entry(menuItem).Property(x=>x.Description).IsModified=change.Description != item.Description;
                    _dbContext.Entry(menuItem).Property(x=>x.IsAvailable).IsModified=change.IsAvailable!=item.IsAvailable;
                    _dbContext.Entry(menuItem).Property(x=>x.Discount).IsModified=change.Discount!=item.Discount;
                    _dbContext.Entry(menuItem).Property(x=>x.DiscountCondiction).IsModified=change.DiscountCondiction!=item.DiscountCondiction;
                }
            }
            return await _dbContext.CompleteAsync();
        }
    }
}
