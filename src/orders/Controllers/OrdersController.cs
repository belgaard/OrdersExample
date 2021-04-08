using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
            if (!IsValid(placeOrderRequest, orderRequest => orderRequest.Id > 0, "Invalid Id", out ActionResult<GenericOrderResponse> actionResult))
                return actionResult;
            if (!IsValid(placeOrderRequest, orderRequest => orderRequest.OrderPrice > 0, "Invalid price", out actionResult))
                return actionResult;
            if (!IsValid(placeOrderRequest, orderRequest => orderRequest.Amount > 0, "Invalid amount", out actionResult))
                return actionResult;
            if (!IsValid(placeOrderRequest, orderRequest => orderRequest.OrderDuration.DurationType == OrderDurationType.GoodTillDate, "Invalid duration", out actionResult)) 
                return actionResult;

            GenericOrderResponse response = await _placeOrderRequestHandler.ProcessAsync(placeOrderRequest);
            return response.ErrorInfo == null ? response : BadRequest(response); // Not brilliant error handling.
        }

        private bool IsValid(PlaceOrderRequest placeOrderRequest, Func<IOrderRequest, bool> checkIsOk, string errorCodeWhenInvalid,
            out ActionResult<GenericOrderResponse> actionResult)
        {
            bool isOk = false;
            if (placeOrderRequest.HasMasterOrder())
                isOk = checkIsOk(placeOrderRequest);
            if (placeOrderRequest.HasRelatedOrders())
                isOk = checkIsOk(placeOrderRequest.Orders.First());

            if (!isOk)
            {
                actionResult = BadRequest(new GenericOrderResponse
                    {ErrorInfo = new ErrorResponse<string> {ErrorCode = errorCodeWhenInvalid}});
                return false;
            }

            actionResult = default;

            return true;
        }
    }
}
