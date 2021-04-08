using Orders.L0Tests.Mocks;
using Orders.Orders.PlaceOrder;
using Orders.SharedDomain;

namespace Orders.L0Tests
{
    public static class TestData
    {
        public const int TheInstrumentIdUsedAllOver = 42;

        public static readonly string InvalidId = Newtonsoft.Json.JsonConvert.SerializeObject(
            new PlaceOrderRequest
            {
                Id = -1,
                Price = 42.0,
                Amount = TheInstrumentIdUsedAllOver,
                OrderType = PlaceableOrderType.TrailingStop, 
                PositionId = "42",
                OrderDuration = 
                    new OrderDuration {DurationType = OrderDurationType.GoodTillDate}, 
                AssetType = AssetType.Stock
            });
        public static readonly string InvalidPrice = Newtonsoft.Json.JsonConvert.SerializeObject(
            new PlaceOrderRequest
            {
                Id = TheInstrumentIdUsedAllOver,
                Price = -42.0,
                Amount = TheInstrumentIdUsedAllOver,
                OrderType = PlaceableOrderType.TrailingStop, 
                PositionId = "42",
                OrderDuration = 
                    new OrderDuration {DurationType = OrderDurationType.GoodTillDate}, 
                AssetType = AssetType.Stock
            });
        public static readonly string InvalidAmount = Newtonsoft.Json.JsonConvert.SerializeObject(
            new PlaceOrderRequest
            {
                Id = TheInstrumentIdUsedAllOver,
                Price = 42.0,
                Amount = -TheInstrumentIdUsedAllOver,
                OrderType = PlaceableOrderType.TrailingStop, 
                PositionId = "42",
                OrderDuration = 
                    new OrderDuration {DurationType = OrderDurationType.GoodTillDate}, 
                AssetType = AssetType.Stock
            });
        public static readonly string InvalidDuration = Newtonsoft.Json.JsonConvert.SerializeObject(
            new PlaceOrderRequest
            {
                Id = TheInstrumentIdUsedAllOver,
                Price = 42.0,
                Amount = TheInstrumentIdUsedAllOver,
                OrderType = PlaceableOrderType.TrailingStop, 
                PositionId = "42",
                OrderDuration = 
                    new OrderDuration {DurationType = (OrderDurationType)(-1)}, 
                AssetType = AssetType.Stock
            });
        public static readonly string ValidBuyOrder = Newtonsoft.Json.JsonConvert.SerializeObject(
            new PlaceOrderRequest
            {
                Orders = new PlaceRelatedOrder[]
                {
                    new()
                    {
                        OrderPrice = (decimal?)42.0,
                        Id = TheInstrumentIdUsedAllOver,
                        BuySell = BuySell.Buy,
                        OrderType = PlaceableOrderType.TrailingStop, 
                        AssetType = AssetType.Stock,
                        OrderDuration = new OrderDuration {DurationType = OrderDurationType.GoodTillDate}
                    }
                },
                PositionId = "42",
            });
        public static readonly string ValidSellOrder = Newtonsoft.Json.JsonConvert.SerializeObject(
            new PlaceOrderRequest
            {
                Id = TheInstrumentIdUsedAllOver,
                BuySell = BuySell.Sell,
                Price = 42.0,
                OrderType = PlaceableOrderType.TrailingStop, 
                PositionId = "42",
                OrderDuration = 
                    new OrderDuration {DurationType = OrderDurationType.GoodTillDate}, 
                AssetType = AssetType.Stock
            });
        public static readonly string OrderWithInvalidPositionId = Newtonsoft.Json.JsonConvert.SerializeObject(
            new PlaceOrderRequest
            {
                Id = TheInstrumentIdUsedAllOver,
                BuySell = BuySell.Sell,
                Price = 42.0,
                OrderType = PlaceableOrderType.TrailingStop, 
                PositionId = "Boom",
                OrderDuration = 
                    new OrderDuration {DurationType = OrderDurationType.GoodTillDate}, 
                AssetType = AssetType.Stock
            });

        public static readonly Tradable TradableAsset = new(true, TheInstrumentIdUsedAllOver);
        public static readonly Tradable NonTradableAsset = new(false, TheInstrumentIdUsedAllOver);
    }
}