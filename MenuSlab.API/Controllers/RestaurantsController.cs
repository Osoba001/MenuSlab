using Main.Application.Requests.RestaurantFeatureRequests;
using Main.Application.Requests.RestaurantRequests;
using Main.Application.Requests.RestaurantStaffRequests;
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
    public class RestaurantsController(IMediator mediator, ILogger<CustomControllerBase> logger) : CustomControllerBase(mediator, logger)
    {
        [HttpPost]
        public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantRequest request) => await SendRequestAsync(request);

        [HttpPost("bank-details")]
        public async Task<IActionResult> AddRestaurantAccountDetails([FromBody] AddRestaurantAccountDetailsRequest request)
        {
            request.UserDevice = UserDevice();
            return await SendRequestAsync(request);
        }

        [HttpPut("delete")]
        public async Task<IActionResult> DeleteRestaurant([FromBody] DeleteRestaurantRequest request) => await SendRequestAsync(request);

        [HasPermission(Permission.Administrator)]
        [HttpGet("by-admin/{country}")]
        public async Task<IActionResult> FetchRestaurantsByAdmin(string country, int page, int pageSize) =>
            await SendRequestAsync(new FetchRestaurantsByAdminRequest { Country = country, Page = page, PageSize = pageSize });

        [HttpGet]
        public async Task<IActionResult> FetchRestaurantsByUser() =>
            await SendRequestAsync(new FetchRestaurantsByUserRequest());


        [HttpGet("{country}/{subNumber}")]
        [ProducesResponseType(200, Type = typeof(List<FetchRestaurantResponse>))]
        public async Task<IActionResult> FetchDispatcher(string country, string subNumber, int page, int pageSize) =>
           await SendRequestAsync(new FetchRestaurantsRequest { Page = page, PageSize = pageSize, Country = country, SubNumber = subNumber });

        [HttpGet("by-location")]
        [ProducesResponseType(200, Type = typeof(List<FetchRestaurantByLocationResponse>))]
        public async Task<IActionResult> FetchRestaurantByLocation(decimal latitude, decimal longitude) => await SendRequestAsync(new FetchRestaurantByLocationRequest { Latitude = latitude, Longitude = longitude });

        [HttpGet("{restaurantId}")]
        [ProducesResponseType(200, Type = typeof(GetRestaurantDetailResponse))]
        public async Task<IActionResult> GetRestaurantDetail(Guid restaurantId) =>
           await SendRequestAsync(new GetRestaurantDetailRequest { Id = restaurantId });

        [HttpGet("full-details/{restaurantId}")]
        [ProducesResponseType(200, Type = typeof(GetRestaurantFullDetailResponse))]
        public async Task<IActionResult> GetRestaurantFullDetail(Guid restaurantId) =>
           await SendRequestAsync(new GetRestaurantFullDetailRequest { Id = restaurantId });

        [HasPermission(Permission.Administrator)]
        [HttpPut("lock")]
        public async Task<IActionResult> UpdateLockRestaurantStatus([FromBody] UpdateLockRestaurantStatusRequest request) => await SendRequestAsync(request);

        [HttpPut]
        public async Task<IActionResult> UpdateRestaurant([FromBody] UpdateRestaurantRequest request)
        {
            request.UserDevice = UserDevice();
            return await SendRequestAsync(request);
        }

        [HasPermission(Permission.Administrator)]
        [HttpPut("verify")]
        public async Task<IActionResult> VerifyRestaurant([FromBody] VerifyRestaurantRequest request) => await SendRequestAsync(request);

        //1
        [HttpPost("add-staff")]
        public async Task<IActionResult> AddRestaurantStaff([FromBody] AddRestaurantStaffRequest request) => await SendRequestAsync(request);

        [HttpPut("remove-staff")]
        public async Task<IActionResult> RemoveRestaurantStaff([FromBody] RemoveRestaurantStaffRequest request) => await SendRequestAsync(request);

        [HttpPut("update-staff")]
        public async Task<IActionResult> UpdateRestaurantStaff([FromBody] UpdateRestaurantStaffRequest request) => await SendRequestAsync(request);

        [ProducesResponseType(200, Type = typeof(List<RestaurantStaffResponse>))]
        [HttpGet("staff/{restaurantId}")]
        public async Task<IActionResult> FetchRestaurantStaff(Guid restaurantId) => await SendRequestAsync(new FetchRestaurantStaffRequest { RestaurantId = restaurantId });

        //2
        [HttpPut("add-feature-to-restaurant")]
        public async Task<IActionResult> AddFeatureToRestaurant([FromBody] AddFeatureToRestaurantRequest request) => await SendRequestAsync(request);

        [HttpPut("remove-feature-from-restaurant")]
        public async Task<IActionResult> RemoveFeatureFromRestaurant([FromBody] RemoveFeatureFromRestaurantRequest request) => await SendRequestAsync(request);

        [HasPermission(Permission.Administrator)]
        [HttpPost("upsert-feature")]
        public async Task<IActionResult> UpsertRestaurantFeature([FromBody] UpsertRestaurantFeatureRequest request) => await SendRequestAsync(request);

        [HasPermission(Permission.Administrator)]
        [HttpDelete("delete-feature/{id}")]
        public async Task<IActionResult> UpsertRestaurantFeature(Guid id) => await SendRequestAsync(new DeleteRestaurantFeatureRequest { Id = id });


        [HttpGet("feature/{restaurantId}")]
        [ProducesResponseType(200, Type = typeof(List<RestaurantFeatureResponse>))]
        public async Task<IActionResult> FetchRestaurantFeatureByRestaurant(Guid restaurantId) => await SendRequestAsync(new FetchRestaurantFeatureByRestaurantRequest { RestaurantId = restaurantId });


        [HasPermission(Permission.Administrator)]
        [ProducesResponseType(200, Type = typeof(List<RestaurantFeatureResponse>))]
        [HttpGet("feature")]
        public async Task<IActionResult> FetchRestaurantFeature() => await SendRequestAsync(new FetchRestaurantFeatureRequest());
    }
}
