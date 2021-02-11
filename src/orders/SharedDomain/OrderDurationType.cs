namespace Orders.SharedDomain
{
    /// <summary>An enum describing the different order duration types.
    /// Note that not all order types are possible for all asset types.</summary>
    public enum OrderDurationType
    {
        /// <summary>Good til Date - Expiration Date must also be specified.</summary>
        GoodTillDate
    }
}