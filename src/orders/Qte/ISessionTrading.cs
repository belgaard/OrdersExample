using System.Collections.Generic;
using AutoMapper;

namespace Orders.Qte
{
    internal interface ISessionTrading
    {
        IAsyncEnumerable<OrderRequestResult> Place3WayOrder(PlaceOrderRequest placeOrderRequest);
    }

    public class SessionTrading : ISessionTrading
    {
        private readonly Tbl.Protos.Tbl.TblClient _tblClient;

        public SessionTrading(Tbl.Protos.Tbl.TblClient tblClient, IMapper mapper)
        {
            _tblClient = tblClient;
        }

        public async IAsyncEnumerable<OrderRequestResult> Place3WayOrder(PlaceOrderRequest placeOrderRequest)
        {
            var tblRequest = new Tbl.Protos.PlaceOrderRequest {Uic = placeOrderRequest.Uic}; // TODO: Convert or map!
            var retVal = await _tblClient.Place3WayOrderAsync(tblRequest);
            foreach (Tbl.Protos.OrderRequestResult result in retVal.Result)
                yield return new OrderRequestResult {IsRejected = result.IsRejected, OrderId = result.OrderId.Value }; // TODO: Convert or map!
        }
    }
}