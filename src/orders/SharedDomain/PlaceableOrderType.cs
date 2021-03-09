namespace Orders.SharedDomain
{
    /// <summary>Specifies supported order types placeable in the orders ticket.</summary>
    public enum PlaceableOrderType
    {
        /// <summary>Market Order.</summary>
        Market = 1,
        /// <summary>Limit Order.</summary>
        Limit = 2,
        /// <summary>Trailing stop.</summary>
        TrailingStop = 9,
    }
}