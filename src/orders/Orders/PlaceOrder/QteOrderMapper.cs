using System;
using System.Linq;
using Orders.Infrastructure.Extensions;
using Orders.Qte;
using Orders.SharedDomain;

namespace Orders.Orders.PlaceOrder
{
    public class QteOrderMapper
    {
        public Qte.PlaceOrderRequest ToQuoteEnginePlaceOrderRequest(PlaceOrderRequest request)
        {
            if (request.HasMasterOrder())
            {
                PlaceOrderRequestBuilder builder = new PlaceOrderRequestBuilder();
                if (request.HasRelatedOrders())
                {
                    builder.BuildBaseOrder(request.Orders[0], OpenOrderRelation.IfDoneMaster);

                    foreach (var relatedOrder in request.Orders)
                    {
                        builder.AddRelatedOrder(relatedOrder, OpenOrderRelation.IfDoneSlave);
                    }

                    var placeOrderRequest = builder.Build();

                    return placeOrderRequest;
                }

                // Only a master order
                builder.BuildBaseOrder(request, OpenOrderRelation.StandAlone);

                return builder.Build();
            }
            if (request.HasRelatedOrders())
            {
                var baseOrder = request.Orders.First();

                var builder = new PlaceOrderRequestBuilder();

                // Request for placing related orders to existing order or position
                builder.BuildBaseOrder(baseOrder, OpenOrderRelation.StandAlone);

                Qte.PlaceOrderRequest result = builder.Build();

                if (request.PositionId.AsNullableLong().HasValue)
                    result.RelatedPositionId = request.PositionId.AsNullableLong();

                return result;
            }

            throw new NotSupportedException("Request is neither a master order, or related orders to existing position/order");
        }

        public static OrderExpireData? MapToOrderExpireData(OrderDuration orderDuration)
        {
            if (orderDuration == null) throw new ArgumentNullException(nameof(orderDuration));

            return new OrderExpireData(orderDuration.ExpirationDateTime.GetValueOrDefault());
        }
    }
}