using Main.Application.Requests.MenuRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.MenuHandlers
{
    internal class DeleteMenuCategoriesHandler(MenuslabDbContext dbContext):IRequestHandler<DeleteMenuCategoriesRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(DeleteMenuCategoriesRequest request, CancellationToken cancellationToken = default)
        {
            var isOwner = await _dbContext.Restaurants
            .AnyAsync(x => x.Id == request.RestaurantId && x.UserId == request.UserIdentifier);
            if (!isOwner) return Forbiden("This is not your restaurant");

            var categories = await _dbContext.MenuCategories
                .Where(x => x.RestaurantId == request.RestaurantId &&
                            (request.DeleteCategoryIds.Contains(x.Id) || request.UpdatedMenuCategories.Select(c => c.Id).Contains(x.Id)))
                .ToListAsync();

            var deleteCategories = categories.Where(x => request.DeleteCategoryIds.Contains(x.Id)).ToList();
            var updateCategories = categories.Where(x => request.UpdatedMenuCategories.Select(c => c.Id).Contains(x.Id)).ToDictionary(x => x.Id, x => x);

            _dbContext.MenuCategories.RemoveRange(deleteCategories);

            foreach (var cat in request.UpdatedMenuCategories)
            {
                if (updateCategories.TryGetValue(cat.Id, out var menuCategory))
                {
                    menuCategory.Index = cat.Index;
                    _dbContext.Entry(menuCategory).Property(x => x.Index).IsModified = true;
                }
            }

            return await _dbContext.CompleteAsync();

        }
    }
}
