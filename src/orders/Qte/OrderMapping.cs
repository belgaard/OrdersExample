using System;
using Orders.SharedDomain;

namespace Orders.Qte
{
    public static class OrderMapping
    {
        /// <summary>Maps PlaceableOrderType to the external <see cref="OpenOrderType"/>.</summary>
        public static OpenOrderType MapOrderType(PlaceableOrderType? orderType)
        {
            switch (orderType.GetValueOrDefault())
            {
                case PlaceableOrderType.Market:
                    return OpenOrderType.Market;
                case PlaceableOrderType.Limit:
                    return OpenOrderType.Limit;
                case PlaceableOrderType.TrailingStop:
                    return OpenOrderType.Stop;
                default:
                    throw new ArgumentOutOfRangeException("Cannot map orderType '" + orderType.GetValueOrDefault() + "' to FrontOffice.Trading.Public.OpenOrderType.");
            }
        }
        /// <summary>Maps our OrderDuration to external <see cref="OpenOrderDuration"/>.</summary>
        public static OpenOrderDuration? MapOrderDuration(OrderDuration orderDuration)
        {
            if (orderDuration == null) throw new ArgumentNullException(nameof(orderDuration));

            switch (orderDuration.DurationType)
            {
                case OrderDurationType.GoodTillDate:
                    return OpenOrderDuration.GoodTillDate;
                default:
                    throw new ArgumentOutOfRangeException("Cannot map orderDuration.DurationType '" + orderDuration.DurationType + "' to FrontOffice.Trading.Public.OpenOrderDuration");
            }
        }
    }
}