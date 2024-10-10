using Main.Application.Requests.MenuRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.MenuHandlers
{
    internal class DeleteMenuItemsHandler(MenuslabDbContext dbContext) : IRequestHandler<DeleteMenuItemsRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(DeleteMenuItemsRequest request, CancellationToken cancellationToken = default)
        {
            var isOwner = await _dbContext.Restaurants
            .AnyAsync(x => x.Id == request.RestaurantId && x.UserId == request.UserIdentifier);
            if (!isOwner) return Forbiden("This is not your restaurant");


            var items = await _dbContext.MenuItems
                .Where(x =>x.Category.RestaurantId == request.RestaurantId && request.DeleteItemIds.Contains(x.Id) || request.UpdatedMenuItems.Select(c => c.Id).Contains(x.Id))
                .ToListAsync(cancellationToken);

            var deleteItems = items.Where(x => request.DeleteItemIds.Contains(x.Id)).ToList();
            var updateItems = items.Where(x => request.UpdatedMenuItems.Select(c => c.Id).Contains(x.Id)).ToDictionary(x => x.Id, x => x);

            _dbContext.MenuItems.RemoveRange(deleteItems);

            foreach (var itm in request.UpdatedMenuItems)
            {
                if (updateItems.TryGetValue(itm.Id, out var menuItem))
                {
                    menuItem.Index = itm.Index;
                    _dbContext.Entry(menuItem).Property(x => x.Index).IsModified = true;
                }
            }

            return await _dbContext.CompleteAsync();
        }
    }
}
