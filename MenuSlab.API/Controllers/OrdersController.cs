using Main.Application.Requests.OfferOrderRequests;
using Main.Application.Requests.OrderRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Share.MediatKO;

namespace MenuSlab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController(IMediator mediator, ILogger<CustomControllerBase> logger) : CustomControllerBase(mediator, logger)
    {

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request) => await SendRequestAsync(request);

        [HttpPost("add-item")]
        public async Task<IActionResult> AddItemToOrder([FromBody] AddItemToOrderRequest request) => await SendRequestAsync(request);

        [HttpPut("remove-item")]
        public async Task<IActionResult> RemoveItemFromOrder([FromBody] RemoveItemFromOrderRequest request) => await SendRequestAsync(request);

        [HttpPut("add-dispatcher")]
        public async Task<IActionResult> AddDispatcherToOrder([FromBody] AddDispatcherToOrderRequest request) => await SendRequestAsync(request);

        [HttpPut("add-payment")]
        public async Task<IActionResult> AddPaymentToOrder([FromBody] AddPaymentToOrderRequest request) => await SendRequestAsync(request);

        [HttpPut]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusByRestaurantRequest request) => await SendRequestAsync(request);

        [HttpPost("rate")]
        public async Task<IActionResult> RateOrder([FromBody] RateOrderRequest request) => await SendRequestAsync(request);

        [HttpPut("complete-order")]
        public async Task<IActionResult> CompleteOrder([FromBody] CompleteOrderRequest request) => await SendRequestAsync(request);

        [HttpGet("restaurant/{restaurantId}")]
        [ProducesResponseType(200, Type = typeof(List<FetchOrderByRestaurantResponse>))]
        public async Task<IActionResult> FetchOrderByResturantOrder(Guid restaurantId, int page, int pageSize) => await SendRequestAsync(new FetchOrderByRestaurantRequest { RestaurantId = restaurantId, Page = page, PageSize = pageSize });

        [HttpGet("dispatcher/{dispatcherId}")]
        [ProducesResponseType(200, Type = typeof(List<FetchOrderByDispatcherResponse>))]
        public async Task<IActionResult> FetchOrderByDispatcherOrder(Guid dispatcherId, int page, int pageSize) => await SendRequestAsync(new FetchOrderByDispatcherRequest { DispatcherId = dispatcherId, Page = page, PageSize = pageSize });

        [HttpGet("user")]
        [ProducesResponseType(200, Type = typeof(List<FetchOrderResponse>))]
        public async Task<IActionResult> FetchOrderByUser() => await SendRequestAsync(new FetchOrderByUserRequest());

        [HttpGet("{orderId}")]
        [ProducesResponseType(200, Type = typeof(GetOrderDetailsResponse))]
        public async Task<IActionResult> GetOrder(Guid orderId) => await SendRequestAsync(new GetOrderDetailRequest { Id = orderId });

        [ProducesResponseType(200, Type = typeof(List<FetchOfferOrderResponse>))]
        [HttpGet("offers")]
        public async Task<IActionResult> FetchOfferOrder() => await SendRequestAsync(new FetchOfferOrderRequest());

        [HttpPost("request-offer")]
        public async Task<IActionResult> CreateOfferOrder([FromBody] CreateOfferOrderRequest request) => await SendRequestAsync(request);

        [HttpPut("respond-to-offer")]
        public async Task<IActionResult> RespondOfferOrder([FromBody] RespondOfferOrderRequest request) => await SendRequestAsync(request);

        [HttpDelete("offer-order{id}")]
        public async Task<IActionResult> DeleteOfferOder(Guid id) => await SendRequestAsync(new DeleteOfferOrderRequest { Id = id });
    }

}
