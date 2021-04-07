using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orders.Orders;
using Orders.Orders.PlaceOrder;
using Orders.SharedDomain;

namespace Orders.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly PlaceOrderRequestHandler _placeOrderRequestHandler;

        public OrdersController(ILogger<OrdersController> logger, PlaceOrderRequestHandler placeOrderRequestHandler)
        {
            _logger = logger;
            _placeOrderRequestHandler = placeOrderRequestHandler;
        }

        [HttpPost]
        [Route("place-order")]
        public async Task<ActionResult<GenericOrderResponse>> PlaceOrderAsync([Required, FromBody] PlaceOrderRequest placeOrderRequest)
        {
            // Simple input validation:
            if (placeOrderRequest.Id < 1)
                return BadRequest(new GenericOrderResponse {ErrorInfo = new ErrorResponse<string> {ErrorCode =  "Invalid Id"}});
            if (placeOrderRequest.Price < 0)
                return BadRequest(new GenericOrderResponse {ErrorInfo = new ErrorResponse<string> {ErrorCode =  "Invalid price"}});
            if (placeOrderRequest.Amount < 0)
                return BadRequest(new GenericOrderResponse {ErrorInfo = new ErrorResponse<string> {ErrorCode =  "Invalid amount"}});
            if (placeOrderRequest.OrderDuration.DurationType != OrderDurationType.GoodTillDate)
                return BadRequest(new GenericOrderResponse {ErrorInfo = new ErrorResponse<string> {ErrorCode =  "Invalid duration"}});

            GenericOrderResponse response = await _placeOrderRequestHandler.ProcessAsync(placeOrderRequest);
            return response.ErrorInfo == null ? response : BadRequest(response); // Not brilliant error handling.
        }
    }
}
