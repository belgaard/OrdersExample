using System.Collections.Generic;
using System.Threading.Tasks;
using Orders.ExternalDependencies;
using Orders.Qte;
using Orders.SharedDomain;

namespace Orders.L0Tests.Mocks
{
    public interface IQtePlacedOrderReader
    {
        (int id, BuySell buySell) Query();
    }

    public class MockSessionTradesFacade : ISessionTradesFacade, IQtePlacedOrderReader
    {
        private (int id, BuySell buySell) _lastPlacedOrder;

        public async IAsyncEnumerable<OrderRequestResult> Place3WayOrder(PlaceOrderRequest placeOrderRequest)
        {
            _lastPlacedOrder = (placeOrderRequest.Uic, placeOrderRequest.BuySell);
            yield return await Task.FromResult(new OrderRequestResult());
        }
        public (int id, BuySell buySell) Query() => _lastPlacedOrder;
    }
}