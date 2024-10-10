using Main.Application.Requests.UserRequests;
using Main.Infrastructure.Authentications.PermissionAuthorizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Share.MediatKO;

namespace MenuSlab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IMediator mediator, ILogger<CustomControllerBase> logger) : CustomControllerBase(mediator, logger)
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreatedSingleMarketApp([FromBody] CreateUserRequest request)
        {
            request.UserDevice = UserDevice();
            return await SendRequestAsync(request);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            request.UserDevice = UserDevice();
            return await SendRequestAsync(request);
        }

        [Authorize]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            request.UserDevice = UserDevice();
            return await SendRequestAsync(request);
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgotPasswordRequest request)
        {
            request.UserDevice = UserDevice();
            return await SendRequestAsync(request);
        }

        [HttpPost("recover-password")]
        public async Task<IActionResult> RecoverPassword([FromBody] RecoveryPasswordRequest request) => await SendRequestAsync(request);

        [HttpPost("set-new-password")]
        public async Task<IActionResult> SetNewPassword([FromBody] SetNewPasswordRequest request) => await SendRequestAsync(request);

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request) => await SendRequestAsync(request);

        [Authorize]
        [HttpGet("user")]
        public async Task<IActionResult> GetById() => await SendRequestAsync(new UserByIdRequest());

        [Authorize]
        [HttpPut("delete")]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserRequest request) => await SendRequestAsync(request);


        [HasPermission(Permission.Administrator)]
        [HttpGet("users/{page}/{pageSize}")]
        public async Task<IActionResult> AllUsers(int page, int pageSize) =>
           await SendRequestAsync(new FetchUsersRequest { Page = page, PageSize = pageSize });

        [HasPermission(Permission.Administrator)]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetById(Guid id) =>
           await SendRequestAsync(new UserByIdRequest { UserIdentifier = id });

        [HasPermission(Permission.Administrator)]
        [HttpGet("user/by-email/{email}")]
        public async Task<IActionResult> GetUser(string email) =>
            await SendRequestAsync(new UserByEmailRequest { Email = email });

        [HttpGet("upload-db")]
        public async Task<IActionResult> UploadDp(IFormFile file) =>
            await SendRequestAsync(new UploadProfilePhotoRequest { File = file });
    }
}
