using Orders.SharedDomain;

namespace Orders.Orders.PlaceOrder
{
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
}