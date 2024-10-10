using Main.Application.Requests.CountryRequests;
using Main.Infrastructure.Database;
using Main.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.CountryHandlers
{
    internal class UpdateCountryPaymentdetailHandler(MenuslabDbContext dbContext) : IRequestHandler<UpdateCountryPaymentdetailRequest>
    {
        private readonly MenuslabDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(UpdateCountryPaymentdetailRequest request, CancellationToken cancellationToken = default)
        {
            var countries = await _dbContext.Countries.Where(x=>request.PaymentDetails.Select(p=>p.Country).Contains(x.Name)).ToDictionaryAsync(x => x.Name.ToLower().Trim(), x => x);
            var newCountries = new List<Country>();
            foreach (var _country in request.PaymentDetails)
            {
                if (countries.TryGetValue(_country.Country.ToLower().Trim(), out var country))
                {
                    country.BaseOrderChange = _country.BaseOrderChange;
                    country.MaxTransfer= _country.MaxTransfer;
                    country.MinTransfer= _country.MinTransfer;
                    country.MaxWithDrawal= _country.MaxWithDrawal;
                    country.MinWithDrawal= _country.MinWithDrawal;
                    _dbContext.Countries.Update(country);
                }
            }
           
            return await _dbContext.CompleteAsync();
        }
    }
}
