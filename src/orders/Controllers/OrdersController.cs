﻿using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orders.Orders;
using Orders.Orders.PlaceOrder;

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
            GenericOrderResponse response = await _placeOrderRequestHandler.ProcessAsync(placeOrderRequest);
            return response.ErrorInfo == null ? response : BadRequest(response); // Not brilliant error handling.
        }
    }
}
