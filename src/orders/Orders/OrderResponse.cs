using Orders.Controllers;
using Orders.SharedDomain;

namespace Orders.Orders
{
    public class OrderResponse
    {
        /// <summary>Id of order. No value provided if request failed.</summary>
        public string OrderId { get; set; }

        /// <summary>Contains error info when request failed.</summary>
        public ErrorResponse<string> ErrorInfo { get; set; }
    }
}