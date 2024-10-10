using Main.Application.Requests.MenuRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.MenuHandlers
{
    internal class UpdateMenuCategoriesHandler(MenuslabDbContext dbContext) : IRequestHandler<UpdateMenuCategoriesRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(UpdateMenuCategoriesRequest request, CancellationToken cancellationToken = default)
        {
            var isOwner = await _dbContext.Restaurants
            .AnyAsync(x => x.Id == request.RestaurantId && x.UserId == request.UserIdentifier);
            if (!isOwner) return Forbiden("This is not your restaurant");

            var existingCat = await _dbContext.MenuCategories.Where(x => request.MenuCategories.Select(cat => cat.Id).Contains(x.Id))
                .ToDictionaryAsync(x => x.Id, x => x);
            
            foreach (var cat in request.MenuCategories)
            {
                if(existingCat.TryGetValue(cat.Id, out var category))
                {
                    var changed= new {category.Name, category.Index};
                    category.Name= cat.Name;
                    category.Index= cat.Index;
                    _dbContext.Entry(category).Property(x=>x.Name).IsModified= changed.Name!=cat.Name;
                    _dbContext.Entry(category).Property(x=>x.Index).IsModified= changed.Index!=cat.Index;
                }
            }
            return await _dbContext.CompleteAsync();
        }
    }
}
