using Orders.L0Tests.Mocks;
using Orders.Orders.PlaceOrder;
using Orders.SharedDomain;

namespace Orders.L0Tests
{
    public static class TestData
    {
        public static readonly string ValidBuyOrder = Newtonsoft.Json.JsonConvert.SerializeObject(
            new PlaceOrderRequest
            {
                Id = 42,
                OrderType = PlaceableOrderType.Limit, 
                OrderDuration = 
                    new OrderDuration {DurationType = OrderDurationType.GoodTillDate}, 
                AssetType = AssetType.Stock
            });

        public static readonly Tradable TradableAsset = new(true, 42);
        public static readonly Tradable NonTradableAsset = new(false, 42);
    }
}