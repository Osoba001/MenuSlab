
using Main.Application.Requests.PaymentGatewayRequests;
using Main.Infrastructure.Authentications.PermissionAuthorizations;
using Microsoft.AspNetCore.Mvc;
using Share.MediatKO;

namespace MenuSlab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController(IMediator mediator, ILogger<CustomControllerBase> logger) : CustomControllerBase(mediator, logger)
    {
        [HasPermission(Permission.Administrator)]
        [HttpPost]
        public async Task<IActionResult> AddPaymentMethodToCountry([FromBody] AddPaymentGatewayRequest request) => await SendRequestAsync(request);

        [HasPermission(Permission.Administrator)]
        [HttpPut]
        public async Task<IActionResult> AddBanks([FromBody] AddBankRequest request) => await SendRequestAsync(request);

        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(List<PaymentGatewayResponse>))]
        public async Task<IActionResult> FetchPaymentGateway() => await SendRequestAsync(new FetchPaymentGatewayRequest());
        
        [HttpGet("{country}")]
        [ProducesResponseType(200, Type = typeof(List<BankResponse>))]
        public async Task<IActionResult> FetchBanks(string country) => await SendRequestAsync(new FetchBankRequest { Country = country });
    }
}
