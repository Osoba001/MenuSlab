using Main.Application.Requests.MenuRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.MenuHandlers
{
    internal class FetchMenuHandler(MenuslabDbContext dbContext) : IRequestHandler<FetchMenuRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(FetchMenuRequest request, CancellationToken cancellationToken = default)
        {
            return new ActionResponse
            {
                PayLoad = await _dbContext.MenuCategories.Where(x => x.RestaurantId == request.RestaurantId)
                .Select(x => new FetchMenuResponse
                {
                    CategoryName = x.Name,
                    IsRoot = x.ParentCategoryId != null ? false : true,
                    ParentCategoryId = x.ParentCategoryId,
                    Index = x.Index,
                    Id = x.Id,
                    Items = x.Items.Select(itm => new MenuItemResponse
                    {
                        Description = itm.Description,
                        Name = itm.Name,
                        PackagePrice = itm.PackagePrice,
                        Price = itm.Price,
                        Index = itm.Index,
                        Discount = itm.Discount,
                        DiscountCondiction = itm.DiscountCondiction,
                        IsAvailable = itm.IsAvailable,
                        Id = itm.Id,
                    }).OrderBy(x => x.Index).ToList()
                }).ToListAsync()
            };
        }
    }
}
