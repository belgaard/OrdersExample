using System.Collections.Generic;
using Orders.Qte;
using Orders.SharedDomain;
using PlaceQteOrderRequest = Orders.Qte.PlaceOrderRequest;

namespace Orders.Orders.PlaceOrder
{
    internal class PlaceOrderRequestBuilder
    {
        private PlaceQteOrderRequest _qteRequest;
        private readonly List<PlaceRelatedOrderRequest> _relatedOrderRequests;

        public PlaceOrderRequestBuilder()
        {
            _qteRequest = new PlaceQteOrderRequest();
            _relatedOrderRequests = new List<PlaceRelatedOrderRequest>();
        }

        public PlaceOrderRequestBuilder BuildBaseOrder(IOrderRequest mainOrder, OpenOrderRelation orderRelation)
        {
            _qteRequest.OrderExpireData = QteOrderMapper.MapToOrderExpireData(mainOrder.OrderDuration);
            _qteRequest.OrderPriceLevel2 = (double?) mainOrder.StopLimitPrice;
            _qteRequest.BuySell = mainOrder.BuySell.GetValueOrDefault();
            _qteRequest.Price = (double) mainOrder.OrderPrice.GetValueOrDefault();
            _qteRequest.InstrumentType = mainOrder.AssetType.ToInstrumentType();
            _qteRequest.Uic = mainOrder.Id.GetValueOrDefault();
            _qteRequest.Amount = (double) mainOrder.Amount.GetValueOrDefault();
            _qteRequest.OrderType = OrderMapping.MapOrderType(mainOrder.OrderType);
            _qteRequest.OpenOrderRelation = orderRelation;
            _qteRequest.OrderDuration = OrderMapping.MapOrderDuration(mainOrder.OrderDuration).GetValueOrDefault();

            if (mainOrder.TrailingStopDistanceToMarket.HasValue && mainOrder.TrailingStopStep.HasValue)
            {
                _qteRequest.TrailingStop = new TrailingStop(mainOrder.TrailingStopDistanceToMarket.Value,
                    mainOrder.TrailingStopStep.Value);
            }

            return this;
        }
        public PlaceQteOrderRequest Build()
        {
            _qteRequest.RelatedOrders = _relatedOrderRequests.ToArray();

            return _qteRequest;
        }

        public PlaceOrderRequestBuilder AddRelatedOrder(IOrderRequest request, OpenOrderRelation orderRelation)
        {
            var result = new PlaceRelatedOrderRequest
            {
                OrderType = OrderMapping.MapOrderType(request.OrderType),
                OrderExpireData = QteOrderMapper.MapToOrderExpireData(request.OrderDuration),
                BuySell = request.BuySell.GetValueOrDefault(),
                Amount = (double)request.Amount.GetValueOrDefault(),
                Price = (double)request.OrderPrice.GetValueOrDefault(),
                OrderPriceLevel2 = (double?)request.StopLimitPrice,
                OpenOrderRelation = orderRelation,
                OrderDuration = OrderMapping.MapOrderDuration(request.OrderDuration).GetValueOrDefault()
            };

            if (request.TrailingStopDistanceToMarket.HasValue && request.TrailingStopStep.HasValue)
            {
                result.TrailingStop = new TrailingStop(request.TrailingStopDistanceToMarket.Value, request.TrailingStopStep.Value);
            }

            _relatedOrderRequests.Add(result);

            return this;
        }
    }
}