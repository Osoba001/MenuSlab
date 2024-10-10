using Main.Application.Requests.SystemAdminRequests;
using Main.Infrastructure.Authentications.PermissionAuthorizations;
using Microsoft.AspNetCore.Mvc;
using Share.MediatKO;

namespace MenuSlab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [HasPermission(Permission.Administrator)]
    public class AdminsController(IMediator mediator, ILogger<CustomControllerBase> logger) : CustomControllerBase(mediator, logger)
    {
        [HttpPost]
        [HasPermission(Permission.SuperAdministrator)]
        public async Task<IActionResult> CreateSystemAdmin([FromBody] CreateSystemAdminRequest request) => await SendRequestAsync(request);

        [HttpPut]
        [HasPermission(Permission.SuperAdministrator)]
        public async Task<IActionResult> UpdateSystemAdmin([FromBody] UpdateSystemAdminRequest request) => await SendRequestAsync(request);

        [HttpGet("{country}")]
        [ProducesResponseType(200, Type = typeof(List<FetchSytemAdminResponse>))]
        public async Task<IActionResult> FetchAdmin(string country) => await SendRequestAsync(new FetchSytemAdminRequest { CountryId =country  });

    }
}
