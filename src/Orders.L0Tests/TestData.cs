using Orders.L0Tests.Mocks;
using Orders.Orders.PlaceOrder;
using Orders.SharedDomain;

namespace Orders.L0Tests
{
    public static class TestData
    {
        public static readonly string InvalidId = Newtonsoft.Json.JsonConvert.SerializeObject(
            new PlaceOrderRequest
            {
                Id = -1,
                Price = 42.0,
                Amount = 42,
                OrderType = PlaceableOrderType.Limit, 
                OrderDuration = 
                    new OrderDuration {DurationType = OrderDurationType.GoodTillDate}, 
                AssetType = AssetType.Stock
            });
        public static readonly string InvalidPrice = Newtonsoft.Json.JsonConvert.SerializeObject(
            new PlaceOrderRequest
            {
                Id = 42,
                Price = -42.0,
                Amount = 42,
                OrderType = PlaceableOrderType.Limit, 
                OrderDuration = 
                    new OrderDuration {DurationType = OrderDurationType.GoodTillDate}, 
                AssetType = AssetType.Stock
            });
        public static readonly string InvalidAmount = Newtonsoft.Json.JsonConvert.SerializeObject(
            new PlaceOrderRequest
            {
                Id = 42,
                Price = 42.0,
                Amount = -42,
                OrderType = PlaceableOrderType.Limit, 
                OrderDuration = 
                    new OrderDuration {DurationType = OrderDurationType.GoodTillDate}, 
                AssetType = AssetType.Stock
            });
        public static readonly string InvalidDuration = Newtonsoft.Json.JsonConvert.SerializeObject(
            new PlaceOrderRequest
            {
                Id = 42,
                Price = 42.0,
                Amount = 42,
                OrderType = PlaceableOrderType.Limit, 
                OrderDuration = 
                    new OrderDuration {DurationType = (OrderDurationType)(-1)}, 
                AssetType = AssetType.Stock
            });
        public static readonly string ValidBuyOrder = Newtonsoft.Json.JsonConvert.SerializeObject(
            new PlaceOrderRequest
            {
                Id = 42,
                BuySell = BuySell.Buy,
                Price = 42.0,
                OrderType = PlaceableOrderType.Limit, 
                OrderDuration = 
                    new OrderDuration {DurationType = OrderDurationType.GoodTillDate}, 
                AssetType = AssetType.Stock
            });
        public static readonly string ValidSellOrder = Newtonsoft.Json.JsonConvert.SerializeObject(
            new PlaceOrderRequest
            {
                Id = 42,
                BuySell = BuySell.Sell,
                Price = 42.0,
                OrderType = PlaceableOrderType.Limit, 
                OrderDuration = 
                    new OrderDuration {DurationType = OrderDurationType.GoodTillDate}, 
                AssetType = AssetType.Stock
            });

        public static readonly Tradable TradableAsset = new(true, 42);
        public static readonly Tradable NonTradableAsset = new(false, 42);
    }
}