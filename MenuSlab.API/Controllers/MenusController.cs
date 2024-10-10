using Main.Application.Requests.MenuRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Share.MediatKO;

namespace MenuSlab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MenusController(IMediator mediator, ILogger<CustomControllerBase> logger) : CustomControllerBase(mediator, logger)
    {
        [HttpPost("categories")]
        public async Task<IActionResult> UpsertMenuCategories([FromBody] UpdateMenuCategoriesRequest request) => await SendRequestAsync(request);

        //upload itemphoto
        [HttpPost("items")]
        public async Task<IActionResult> UpsertMenuItemRequest([FromBody] UpdateMenuCategoriesRequest request) => await SendRequestAsync(request);

        [HttpGet("{restaurantId}")]
        [ProducesResponseType(200, Type = typeof(List<FetchMenuResponse>))]
        public async Task<IActionResult> FetchMenu(Guid restaurantId) => await SendRequestAsync(new FetchMenuRequest { RestaurantId = restaurantId });

        [HttpDelete("category")]
        public async Task<IActionResult> DeleteMenuCategory([FromBody] DeleteMenuCategoriesRequest request) => await SendRequestAsync(request);

        [HttpPut("item")]
        public async Task<IActionResult> DeleteMenuItems([FromBody] DeleteMenuItemsRequest request) => await SendRequestAsync(request);
    }
}
