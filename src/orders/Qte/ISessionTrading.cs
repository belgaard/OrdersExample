using System.Collections.Generic;

namespace Orders.Qte
{
    internal interface ISessionTrading
    {
        IEnumerable<OrderRequestResult> Place3WayOrder(PlaceOrderRequest placeOrderRequest);
    }

    public class SessionTrading : ISessionTrading
    {
        public IEnumerable<OrderRequestResult> Place3WayOrder(PlaceOrderRequest placeOrderRequest)
        {
            return new List<OrderRequestResult>(); // TODO: Eventually, this will call TBL through gRPC!
        }
    }
}