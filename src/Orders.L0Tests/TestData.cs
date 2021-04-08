using Orders.L0Tests.Mocks;
using Orders.Orders.PlaceOrder;
using Orders.SharedDomain;

namespace Orders.L0Tests
{
    public static class TestData
    {
        public const int ValidInstrumentId = 42;

        public static class RelatedOrders
        {
            public static readonly string InvalidId = Newtonsoft.Json.JsonConvert.SerializeObject(
                new PlaceOrderRequest
                {
                    Orders = new PlaceRelatedOrder[]
                    {
                        new()
                        {
                            Id = -1,
                            OrderPrice = 42.0M,
                            Amount = 42,
                            OrderType = PlaceableOrderType.TrailingStop,
                            OrderDuration =
                                new OrderDuration {DurationType = OrderDurationType.GoodTillDate},
                            AssetType = AssetType.Stock
                        }
                    },
                    PositionId = "42"
                });

            public static readonly string InvalidPrice = Newtonsoft.Json.JsonConvert.SerializeObject(
                new PlaceOrderRequest
                {
                    Orders = new PlaceRelatedOrder[]
                    {
                        new()
                        {
                            Id = ValidInstrumentId,
                            OrderPrice = -42.0M,
                            Amount = 42,
                            OrderType = PlaceableOrderType.TrailingStop,
                            OrderDuration =
                                new OrderDuration {DurationType = OrderDurationType.GoodTillDate},
                            AssetType = AssetType.Stock
                        }
                    },
                    PositionId = "42"
                });

            public static readonly string InvalidAmount = Newtonsoft.Json.JsonConvert.SerializeObject(
                new PlaceOrderRequest
                {
                    Orders = new PlaceRelatedOrder[]
                    {
                        new()
                        {
                            Id = ValidInstrumentId,
                            OrderPrice = 42.0M,
                            Amount = -ValidInstrumentId,
                            OrderType = PlaceableOrderType.TrailingStop,
                            OrderDuration =
                                new OrderDuration {DurationType = OrderDurationType.GoodTillDate},
                            AssetType = AssetType.Stock
                        }
                    },
                    PositionId = "42"
                });

            public static readonly string InvalidDuration = Newtonsoft.Json.JsonConvert.SerializeObject(
                new PlaceOrderRequest
                {
                    Orders = new PlaceRelatedOrder[]
                    {
                        new()
                        {
                            Id = ValidInstrumentId,
                            OrderPrice = 42.0M,
                            Amount = 42,
                            OrderType = PlaceableOrderType.TrailingStop,
                            OrderDuration =
                                new OrderDuration {DurationType = (OrderDurationType) (-1)},
                            AssetType = AssetType.Stock,
                        }
                    },
                    PositionId = "42"
                });

            public static readonly string ValidBuy = Newtonsoft.Json.JsonConvert.SerializeObject(
                new PlaceOrderRequest
                {
                    Orders = new PlaceRelatedOrder[]
                    {
                        new()
                        {
                            OrderPrice = 42.0M,
                            Amount = 42,
                            Id = ValidInstrumentId,
                            BuySell = BuySell.Buy,
                            OrderType = PlaceableOrderType.TrailingStop,
                            AssetType = AssetType.Stock,
                            OrderDuration = new OrderDuration {DurationType = OrderDurationType.GoodTillDate}
                        }
                    },
                    PositionId = "42"
                });

            public static readonly string ValidSell = Newtonsoft.Json.JsonConvert.SerializeObject(
                new PlaceOrderRequest
                {
                    Orders = new PlaceRelatedOrder[]
                    {
                        new()
                        {
                            Id = ValidInstrumentId,
                            Amount = 42,
                            BuySell = BuySell.Sell,
                            OrderPrice = 42.0M,
                            OrderType = PlaceableOrderType.TrailingStop,
                            OrderDuration =
                                new OrderDuration {DurationType = OrderDurationType.GoodTillDate},
                            AssetType = AssetType.Stock
                        }
                    },
                    PositionId = "42"
                });

            public static readonly string InvalidPositionId = Newtonsoft.Json.JsonConvert.SerializeObject(
                new PlaceOrderRequest
                {
                    Orders = new PlaceRelatedOrder[]
                    {
                        new()
                        {
                            Id = ValidInstrumentId,
                            Amount = 42,
                            BuySell = BuySell.Sell,
                            OrderPrice = 42.0M,
                            OrderType = PlaceableOrderType.TrailingStop,
                            OrderDuration =
                                new OrderDuration {DurationType = OrderDurationType.GoodTillDate},
                            AssetType = AssetType.Stock
                        }
                    },
                    PositionId = "Boom"
                });
        }

        public static readonly Tradable TradableAsset = new(true, ValidInstrumentId);
        public static readonly Tradable NonTradableAsset = new(false, ValidInstrumentId);
    }
}