using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace orders.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Orders : ControllerBase
    {
        private readonly ILogger<Orders> _logger;
        private readonly PlaceOrderRequestHandler _placeOrderRequestHandler;

        public Orders(ILogger<Orders> logger, PlaceOrderRequestHandler placeOrderRequestHandler)
        {
            _logger = logger;
            _placeOrderRequestHandler = placeOrderRequestHandler;
        }

        [HttpPost]
        public async Task<ActionResult<OrderResponse>> PlaceOrderAsync([Required, FromBody] PlaceOrderRequest placeOrderRequest)
        {
            return await _placeOrderRequestHandler.ProcessAsync(placeOrderRequest);
        }
    }

    public class PlaceOrderRequestHandler
    {
        public async Task<OrderResponse> ProcessAsync(PlaceOrderRequest request)
        {
            await Task.Delay(1);
            return new OrderResponse {OrderId = "42"};
        }
    }

    /// <summary>Describes the direction of an order, action or trade.</summary>
    public enum BuySell
    {
        /// <summary>Buy.</summary>
        Buy,
        /// <summary>Sell.</summary>
        Sell,
    }
    /// <summary>Specifies supported order types placeable in the orders ticket.</summary>
    public enum PlaceableOrderType
    {
        /// <summary>Market Order.</summary>
        Market = 1,
        /// <summary>Limit Order.</summary>
        Limit = 2
    }
      /// <summary>The possible AssetTypes for which you can get a quote or place an order or a trade.</summary>
      [Serializable]
      public enum AssetType
      {
        /// <summary>Stock.</summary>
        Stock = 1,
        /// <summary>Cfd on Stock.</summary>
        CfdOnStock = 2
      }

    /// <summary>The time frame during which the order is valid. In this simplified example DurationType is always GoodTillDate, so an ExpirationDateTime must be provided.
    /// </summary>
    public sealed class OrderDuration
    {
        /// <summary>The field must always be expressed in the exchange local time.</summary>
        public DateTime? ExpirationDateTime { get; set; }
    }

    /// <summary>Request contract for "Place order"</summary>
    /// <remarks>Note that this is a very simplified request. Among the many simplifications, it is assumed that the caller has a single account, that the duration is
    /// always GoodTilDate etc.</remarks>
    public class PlaceOrderRequest
    {
        /// <summary>Unique id of the instrument to place the order for.</summary>
        public int? Id { get; set; }
        /// <summary>Order size.</summary>
        public decimal? Amount { get; set; }
        /// <summary>The direction of the order; buy or sell.</summary>
        public BuySell? BuySell { get; set; }
        /// <summary>Order Price. Optional for market orders.</summary>
        public decimal? OrderPrice { get; set; }
        /// <summary>Order type.</summary>
        public PlaceableOrderType? OrderType { get; set; }
        /// <summary>The Instruments AssetType.</summary>
        public AssetType? AssetType { get; set; }
        /// <summary>The Order Duration.</summary>
        public OrderDuration OrderDuration { get; set; }
        /// <summary>Used when placing related orders for an existing position.</summary>
        public string PositionId { get; set; }
        /// <summary>Stop limit price for Stop Limit order.</summary>
        public decimal? StopLimitPrice { get; set; }
        /// <summary>Distance to market for a trailing stop order.</summary>
        public decimal? TrailingStopDistanceToMarket { get; set; }
        /// <summary>Step size for trailing stop order.</summary>
        public decimal? TrailingStopStep { get; set; }
    }


    public class OrderResponse
    {
        /// <summary>Id of order. No value provided if request failed.</summary>
        public string OrderId { get; set; }

        /// <summary>Contains error info when request failed.</summary>
        public ErrorResponse<string> ErrorInfo { get; set; }
    }
    /// <summary>Generic minimum error response from any service returning a 400- 40x HTTP Error Code</summary>
    public class ErrorResponse<T>
    {
        /// <summary>Internal error from Qte.</summary>
        internal string InternalError { get; set; }
        /// <summary>ErrorCode</summary>
        public T ErrorCode { get; set; }

        /// <summary>
        ///     Optional Textual information about the error to aid the developer.
        ///     NOTE: Remember to NOT include sensitive information here, which could help a hacker!
        /// </summary>
        public string Message { get; set; }
    }
}
