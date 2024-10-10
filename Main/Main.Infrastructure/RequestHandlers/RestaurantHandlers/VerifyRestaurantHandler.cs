using Main.Application.Requests.RestaurantRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.RestaurantHandlers
{
    internal class VerifyRestaurantHandler(MenuslabDbContext dbContext) : IRequestHandler<VerifyRestaurantRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(VerifyRestaurantRequest request, CancellationToken cancellationToken = default)
        {
            var admin = await _dbContext.SystemStaff.Where(x => x.UserId == request.UserIdentifier && x.IsActive).FirstOrDefaultAsync();
            if (admin == null) return Forbiden();
            var resturants = await _dbContext.Restaurants.Where(x => request.IdNumbers.Keys.Contains(x.Id)).ToDictionaryAsync(x=>x.Id, x=>x);
            if (resturants.Count==0)
                return NotFoundResult();

            var existingKeys = await _dbContext.Restaurants.Where(x => x.CountryId == admin.CountryId && request.IdNumbers.Values.Contains(x.Number)).ToDictionaryAsync(x => x.Number, x => x.Name);
            var response= new Dictionary<string, string>();
            foreach (var item in request.IdNumbers)
            {
                if(existingKeys.TryGetValue(item.Value, out var existkey))
                {
                    response.Add(item.Value, $"Failed: Already existing in: {existkey}");
                }
                else
                {
                    if(resturants.TryGetValue(item.Key,out var restaurant))
                    {
                        restaurant.Number = item.Value;
                        _dbContext.Entry(restaurant).Property(x=>x.Number).IsModified=true;
                        response.Add(item.Value, "Success");
                    }
                    else
                    {
                        response.Add(item.Value, $"Failed: Invalid Id: {existkey}");
                    }
                }
            }
            return await _dbContext.CompleteAsync(response);
        }
    }
}
