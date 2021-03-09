using System.Linq;
using Orders.Qte;
using Orders.SharedDomain;

namespace Orders.Orders.PlaceOrder
{
    public interface IOrderRequest
    {
        /// <summary>Unique id of the instrument to place the order for.</summary>
        int? Id { get; set; }

        /// <summary>Order size.</summary>
<<<<<<< HEAD
        decimal? Amount { get; set; }
=======
        double? Amount { get; set; }
>>>>>>> b42a692431beaf8cb8f2c4cff901ea1550e9c440

        /// <summary>The direction of the order; buy or sell.</summary>
        BuySell? BuySell { get; set; }

        /// <summary>Order Price. Optional for market orders.</summary>
<<<<<<< HEAD
        decimal? OrderPrice { get; set; }
=======
        double? OrderPrice { get; set; }
>>>>>>> b42a692431beaf8cb8f2c4cff901ea1550e9c440

        /// <summary>Order type.</summary>
        PlaceableOrderType OrderType { get; set; }

        /// <summary>The Instruments AssetType.</summary>
        AssetType? AssetType { get; set; }

        /// <summary>The Order Duration.</summary>
        OrderDuration OrderDuration { get; set; }

        /// <summary>Stop limit price for Stop Limit order.</summary>
<<<<<<< HEAD
        decimal? StopLimitPrice { get; set; }

        /// <summary>Distance to market for a trailing stop order.</summary>
        decimal? TrailingStopDistanceToMarket { get; set; }

        /// <summary>Step size for trailing stop order.</summary>
        decimal? TrailingStopStep { get; set; }
=======
        double? StopLimitPrice { get; set; }

        /// <summary>Distance to market for a trailing stop order.</summary>
        double? TrailingStopDistanceToMarket { get; set; }

        /// <summary>Step size for trailing stop order.</summary>
        double? TrailingStopStep { get; set; }
>>>>>>> b42a692431beaf8cb8f2c4cff901ea1550e9c440
    }

    /// <summary>Request contract for "Place order"</summary>
    /// <remarks>Note that this is a very simplified request. Among the many simplifications, it is assumed that the caller has a single account, that the duration is
    /// always GoodTilDate etc.</remarks>
    public class PlaceOrderRequest : IOrderRequest
    {
        /// <summary>Unique id of the instrument to place the order for.</summary>
        public int? Id { get; set; }
        /// <summary>Order size.</summary>
        public double? Amount { get; set; }
        /// <summary>The direction of the order; buy or sell.</summary>
        public BuySell? BuySell { get; set; }
        /// <summary>Order Price. Optional for market orders.</summary>
        public double? OrderPrice { get; set; }
        /// <summary>Order type.</summary>
        public PlaceableOrderType OrderType { get; set; }
        /// <summary>The Instruments AssetType.</summary>
        public AssetType? AssetType { get; set; }
        /// <summary>The Order Duration.</summary>
        public OrderDuration OrderDuration { get; set; }
        /// <summary>Used when placing related orders for an existing position.</summary>
        public string PositionId { get; set; }
        /// <summary>Stop limit price for Stop Limit order.</summary>
        public double? StopLimitPrice { get; set; }
        /// <summary>Distance to market for a trailing stop order.</summary>
        public double? TrailingStopDistanceToMarket { get; set; }
        /// <summary>Step size for trailing stop order.</summary>
<<<<<<< HEAD
        public decimal? TrailingStopStep { get; set; }
=======
        public double? TrailingStopStep { get; set; }
>>>>>>> b42a692431beaf8cb8f2c4cff901ea1550e9c440

        public PlaceRelatedOrder[] Orders { get; set; }
        public OpenOrderRelation? OpenOrderRelation { get; set; }
        public OrderExpireData? OrderExpireData { get; set; }
        public double? OrderPriceLevel2 { get; set; }
        public double Price { get; set; }
        public InstrumentTypes InstrumentType { get; set; }
        public int Uic { get; set; }

        /// <summary>Returns <c>true</c> if request has a master order.</summary>
        internal bool HasMasterOrder() => OrderType == PlaceableOrderType.Limit;
        /// <summary>Returns <c>true</c> if request has related orders related to an existing position using the see cref="PositionId"/> property.</summary>
        internal bool HasRelatedOrders() => Orders?.Any() == true;
        internal int GetTotalNumberOfOrders()
        {
            int orderCount = 0;

            if (HasMasterOrder())
                orderCount = 1 + (Orders?.Length ?? 0);
            else if (HasRelatedOrders())
                orderCount = Orders?.Length ?? 0;
            return orderCount;
        }
    }
        public class PlaceRelatedOrder : IOrderRequest
        {
            /// <summary>Unique id of the instrument to place the order for.</summary>
            public int? Id { get; set; }
            /// <summary>Order size.</summary>
<<<<<<< HEAD
            public decimal? Amount { get; set; }
            /// <summary>The direction of the order; buy or sell.</summary>
            public BuySell? BuySell { get; set; }
            /// <summary>Order Price. Optional for market orders.</summary>
            public decimal? OrderPrice { get; set; }
=======
            public double? Amount { get; set; }
            /// <summary>The direction of the order; buy or sell.</summary>
            public BuySell? BuySell { get; set; }
            /// <summary>Order Price. Optional for market orders.</summary>
            public double? OrderPrice { get; set; }
>>>>>>> b42a692431beaf8cb8f2c4cff901ea1550e9c440
            /// <summary>Order type.</summary>
            public PlaceableOrderType OrderType { get; set; }
            /// <summary>The Instruments AssetType.</summary>
            public AssetType? AssetType { get; set; }
            /// <summary>The Order Duration.</summary>
            public OrderDuration OrderDuration { get; set; }
            /// <summary>Stop limit price for Stop Limit order</summary>
<<<<<<< HEAD
            public decimal? StopLimitPrice { get; set; }
            /// <summary>Distance to market for a trailing stop order.</summary>
            public decimal? TrailingStopDistanceToMarket { get; set; }
            /// <summary>Step size for trailing stop order.</summary>
            public decimal? TrailingStopStep { get; set; }
=======
            public double? StopLimitPrice { get; set; }
            /// <summary>Distance to market for a trailing stop order.</summary>
            public double? TrailingStopDistanceToMarket { get; set; }
            /// <summary>Step size for trailing stop order.</summary>
            public double? TrailingStopStep { get; set; }
>>>>>>> b42a692431beaf8cb8f2c4cff901ea1550e9c440
        }
}