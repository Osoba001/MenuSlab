using Main.Application.Requests.MenuRequests;
using Main.Infrastructure.Database;
using Main.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.MenuHandlers
{
    internal class CreateMenuCategoriesHandler(MenuslabDbContext dbContext) : IRequestHandler<CreateMenuCategoriesRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(CreateMenuCategoriesRequest request, CancellationToken cancellationToken = default)
        {
            var isOwner = await _dbContext.Restaurants
            .AnyAsync(x => x.Id == request.RestaurantId && x.UserId == request.UserIdentifier);
            if (!isOwner) return Forbiden("This is not your restaurant");

            var categories= new List<MenuCategory>();
            foreach (var cat in request.MenuCategories)
            {
                categories.Add(new MenuCategory { RestaurantId=request.RestaurantId, Name=cat.Name, Index=cat.Index, ParentCategoryId=request.IsRoot?null: request.ParentId });
            }
            _dbContext.MenuCategories.AddRange(categories);
            return await _dbContext.CompleteAsync();

        }
    }
}
