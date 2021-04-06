using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LeanTest.Mock;
using Orders.ExternalDependencies;
using Orders.Qte;

namespace Orders.L0Tests.Mocks
{

    public record Tradable(in bool IsTradable, in int Uic);
    public class MockForDataInstrumentCacheFacade : IInstrumentCacheFacade, IMockForData<Tradable>
    {
        private readonly Dictionary<int, bool> _tradablePerUic = new();

        public bool IsInstrumentTradableByUic(int uic) => _tradablePerUic[uic];

        public void WithData(Tradable data) => _tradablePerUic[data.Uic] = data.IsTradable;

        public void PreBuild() { }
        public void Build(Type type) { }
        public void PostBuild() { }
    }

    public class MockSessionTradesFacade : ISessionTradesFacade
    {
        public async IAsyncEnumerable<OrderRequestResult> Place3WayOrder(PlaceOrderRequest placeOrderRequest)
        {
            yield return await Task.FromResult(new OrderRequestResult());
        }
    }
}