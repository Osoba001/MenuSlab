using Main.Application.Enums;
using Main.Application.Requests.WithdrawalRequests;
using Main.Infrastructure.Authentications.PermissionAuthorizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Share.MediatKO;

namespace MenuSlab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WithdrawalsController(IMediator mediator, ILogger<CustomControllerBase> logger) : CustomControllerBase(mediator, logger)
    {

        [HttpPost]
        public async Task<IActionResult> WithdrawFund([FromBody] WithdrawFundRequest request) => await SendRequestAsync(request);

        [HasPermission(Permission.Administrator)]
        [HttpPut("respond-to-withdrawal")]
        public async Task<IActionResult> AttendToRestaurantWithdraw([FromBody] ApproveWithdrawFundRequest request) => await SendRequestAsync(request);

        [HttpGet("dispatcher/{dispatcherId}")]
        [ProducesResponseType(200, Type = typeof(List<WithdrawalDispatcherResponse>))]
        public async Task<IActionResult> FetchDispatcherWithdraw(Guid dispatcherId, int page,int pageSize) =>
          await SendRequestAsync(new FetchDispatcherWithdrawalsRequest { DispatchertId = dispatcherId, Page=page, PageSize=pageSize });

        [HttpGet("resturant/{resturantId}")]
        [ProducesResponseType(200, Type = typeof(List<WithdrawalDispatcherResponse>))]
        public async Task<IActionResult> FetchResturantWithdraw(Guid resturantId, int page, int pageSize) =>
          await SendRequestAsync(new FetchRestaurantWithdrawalsRequest { RestaurantId = resturantId, Page = page, PageSize = pageSize });

        [HasPermission(Permission.Administrator)]
        [HttpGet("{country}/{subNumber}/{sourceType}")]
        [ProducesResponseType(200, Type = typeof(List<WithdrawalByAdminResponse>))]
        public async Task<IActionResult> FetchWithdrawByAdmin(string country, string subNumber, WithdrawalSourceType sourceType, int page, int pageSize) =>
          await SendRequestAsync(new FetchtWithdrawalByAdminRequest { Country = country, SubNumber=subNumber, SourceType=sourceType, Page = page, PageSize = pageSize });
    }
}
