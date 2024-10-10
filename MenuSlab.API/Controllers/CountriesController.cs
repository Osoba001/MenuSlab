using Main.Application.Requests.CountryRequests;
using Main.Infrastructure.Authentications.PermissionAuthorizations;
using Microsoft.AspNetCore.Mvc;
using Share.MediatKO;

namespace MenuSlab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController(IMediator mediator, ILogger<CustomControllerBase> logger) : CustomControllerBase(mediator, logger)
    {
        [HasPermission(Permission.SuperAdministrator)]
        [HttpPost]
        public async Task<IActionResult> AddCountries([FromBody] AddCountriesRequest request) => await SendRequestAsync(request);

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<CountryResponse>))]
        public async Task<IActionResult> FetchCountries() => await SendRequestAsync(new FetchCountriesRequest());

        [HasPermission(Permission.SuperAdministrator)]
        [HttpPut]
        public async Task<IActionResult> UpdateCountriesPaymentGateway([FromBody] UpdateCountryPaymentdetailRequest request) => await SendRequestAsync(request);

        [HasPermission(Permission.Administrator)]
        [HttpGet("full-datail")]
        [ProducesResponseType(200, Type = typeof(List<CountryFullDetailResponse>))]
        public async Task<IActionResult> FetchCountriesFullDetails()=> await SendRequestAsync(new FetchCountriesFullDetailRequest());
    }
}
