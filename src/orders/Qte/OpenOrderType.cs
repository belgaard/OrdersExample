namespace Orders.Qte
{
    /// <summary>Open order type enum</summary>
    public enum OpenOrderType
    {
        /// <summary>0: Unspecified</summary>
        Unknown = 0,
        /// <summary>1: Market order</summary>
        Market = 1,
        /// <summary>2: Limit order</summary>
        Limit = 2,
        /// <summary>
        /// 6: Stop at price
        /// Stop If Offered for buy orders
        /// Stop If Bid for sell orders
        /// </summary>
        Stop = 6
    }
}