using System;
using Orders.Qte;

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

    /// <summary>
    /// Extension methods for AssetTypeMappings.
    /// </summary>
    public static class AssetTypeMappingExtensions
    {
        /// <summary>Converts AssetType to InstrumentTypes.</summary>
        public static InstrumentTypes ToInstrumentType(this AssetType? @this)
        {
            if (@this == null)
            {
                throw new ArgumentNullException(nameof(@this));
            }

            switch (@this.GetValueOrDefault())
            {
                case AssetType.Stock:
                    return InstrumentTypes.Shares;
                case AssetType.CfdOnStock:
                    return InstrumentTypes.Cfd;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}