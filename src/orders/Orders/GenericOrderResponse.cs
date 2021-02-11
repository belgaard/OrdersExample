using Orders.SharedDomain;

namespace Orders.Orders
{
    public class GenericOrderResponse
    {
        /// <summary>Id of order. No value provided if request failed.</summary>
        public string OrderId { get; set; }

        /// <summary>Contains error info when request failed.</summary>
        public ErrorResponse<string> ErrorInfo { get; set; }

        public RelatedOrder[] Orders { get; set; }

        public class RelatedOrder
        {
            /// <summary>Id of related order. No value provided if placement of order failed.</summary>
            public string OrderId { get; set; }
            /// <summary>Contains error info when placement of order failed.</summary>
            public ErrorResponse<string> ErrorInfo { get; set; }
        }
    }
}