using System;

namespace Orders.SharedDomain
{
    /// <summary>The time frame during which the order is valid. In this simplified example DurationType is always GoodTillDate, so an ExpirationDateTime must be provided.
    /// </summary>
    public sealed class OrderDuration
    {
        /// <summary>The field must always be expressed in the exchange local time.</summary>
        public DateTime? ExpirationDateTime { get; set; }
    }
}