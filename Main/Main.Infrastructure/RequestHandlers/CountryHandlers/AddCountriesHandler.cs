using Main.Application.Requests.CountryRequests;
using Main.Infrastructure.Database;
using Main.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.CountryHandlers
{
    internal class AddCountriesHandler(MenuslabDbContext dbContext) : IRequestHandler<AddCountriesRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(AddCountriesRequest request, CancellationToken cancellationToken = default)
        {
            var existingCountries= await _dbContext.Countries.ToDictionaryAsync(x=>x.Name.ToLower().Trim(),x=>x);
            var newCountries=new List<Country>();
            foreach (var country in request.Countries)
            {
                if(!existingCountries.TryGetValue(country.Name.ToLower().Trim(), out _))
                {
                   newCountries.Add(new Country { Name = country.Name, Currency=country.Currency });
                }
            }
            if (newCountries.Count == 0)
                return new ActionResponse { PayLoad = "No new country was added" };
            _dbContext.Countries.AddRange(newCountries);
            return await _dbContext.CompleteAsync(newCountries.Select(x => new CountryResponse {Name= x.Name,Currency= x.Currency }));
        }
    }
}
