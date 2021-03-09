namespace Orders.SharedDomain
{
    /// <summary>Description of the order relation</summary>
    public enum OpenOrderRelation
    {
        /// <summary>0: Standalone. No relations to other orders.</summary>
        StandAlone,
        /// <summary>
        /// 1: One cancels other. Relation between two orders. When one is filled, the other is cancelled.
        /// </summary>
        Oco,
        /// <summary>
        /// 2: If done master. Relation between two or three orders.
        /// The slave orders are released only if the master order is filled.
        /// </summary>
        IfDoneMaster,
        /// <summary>
        /// 3: If done slave. Relation between two orders. The other is always IfDoneMaster.
        /// </summary>
        IfDoneSlave,
        /// <summary>
        /// 4: If done slave OCO. Relation between three orders.
        /// One is always IfDoneMaster and the other is always also IfDoneSlaveOCO.
        /// The OCO relation is between the two slave orders.
        /// </summary>
        IfDoneSlaveOco,
    }
}