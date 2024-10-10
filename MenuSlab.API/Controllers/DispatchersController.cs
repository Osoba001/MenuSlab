using Main.Application.Requests.DispatcherRequests;
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
    public class DispatchersController(IMediator mediator, ILogger<CustomControllerBase> logger) : CustomControllerBase(mediator, logger)
    {
        [HttpPost]
        public async Task<IActionResult> CreateDispatcher([FromBody] CreateDispatcherRequest request) => await SendRequestAsync(request);

        [HttpPost("bank-account")]
        public async Task<IActionResult> AddBankDispatcher([FromBody] AddDispatcherAccountDetailsRequest request)
        {
            request.UserDevice = UserDevice();
           return await SendRequestAsync(request);
        }

        [HttpPut("delete")]
        public async Task<IActionResult> DeleteDispatcher([FromBody] DeleteDispatcherRequest request) => await SendRequestAsync(request);

        

        [HasPermission(Permission.Administrator)]
        [HttpGet("by-admin/{country}")]
        [ProducesResponseType(200, Type = typeof(List<FetchFullDispatcherResponse>))]
        public async Task<IActionResult> FetchDispatcherByAdmin(string country, int page, int pageSize) =>
            await SendRequestAsync(new FetchDispatchersByAdminRequest { Country = country, Page = page, PageSize = pageSize });

        [HttpGet("{country}/{subNumber}")]
        [ProducesResponseType(200, Type = typeof(List<FetchDispatcherResponse>))]
        public async Task<IActionResult> FetchDispatcher(string country, string subNumber, int page, int pageSize) =>
           await SendRequestAsync(new FetchDispatchersRequest { Page = page, PageSize = pageSize, Country = country, SubNumber = subNumber});


        [HttpGet("{dispatcherId}")]
        [ProducesResponseType(200, Type = typeof(GetDispatcherDetailResponse))]
        public async Task<IActionResult> GetDispatcherDetail(Guid dispatcherId) =>
           await SendRequestAsync(new GetDispatcherDetailRequest { Id = dispatcherId });

        [HttpGet("full-details/{dispatcherId}")]
        [ProducesResponseType(200, Type = typeof(GetDispatcherFullDetailResponse))]
        public async Task<IActionResult> GetDispatcherFullDetail(Guid dispatcherId) =>
           await SendRequestAsync(new GetDispatcherFullDetailRequest { Id = dispatcherId });

        [HttpPut]
        public async Task<IActionResult> UpdateDispatcher([FromBody] UpdateDispatcherRequest request) => await SendRequestAsync(request);

        [HasPermission(Permission.Administrator)]
        [HttpPut("lock")]
        public async Task<IActionResult> UpdateLockDispatcherStatus([FromBody] UpdateLockDispatcherStatusRequest request) => await SendRequestAsync(request);

        [HasPermission(Permission.Administrator)]
        [HttpPut("verify")]
        public async Task<IActionResult> VerifyDispatcher([FromBody] VerifyDispatcherRequest request) => await SendRequestAsync(request);
    }
}
