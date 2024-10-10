using Main.Application.Requests.FundTransactionRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Share.MediatKO;

namespace MenuSlab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FundsController(IMediator mediator, ILogger<CustomControllerBase> logger) : CustomControllerBase(mediator, logger)
    {
        [HttpPut]
        public async Task<IActionResult> TransferFund([FromBody] TransferFundRequest request) => await SendRequestAsync(request);

        [HttpPost]
        public async Task<IActionResult> FundAccount([FromBody] FundAccountRequest request) => await SendRequestAsync(request);

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<TransactionResponse>))]
        public async Task<IActionResult> FetchTransaction(int page, int pageSize) => await SendRequestAsync(new FetchTransactionRequest { Page = page, PageSize = pageSize });
    }
}
