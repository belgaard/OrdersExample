using System;

namespace Orders.SharedDomain
{
    /// <summary>The possible AssetTypes for which you can get a quote or place an order or a trade.</summary>
    [Serializable]
    public enum AssetType
    {
        /// <summary>Stock.</summary>
        Stock = 1,
        /// <summary>Cfd on Stock.</summary>
        CfdOnStock = 2
    }
}