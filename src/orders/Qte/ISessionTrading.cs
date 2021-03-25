using System.Collections.Generic;
using System.Threading.Tasks;
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
        private readonly IMapper _mapper;

        public SessionTrading(Tbl.Protos.Tbl.TblClient tblClient, IMapper mapper)
        {
            _tblClient = tblClient;
            _mapper = mapper;
        }

        public async IAsyncEnumerable<OrderRequestResult> Place3WayOrder(PlaceOrderRequest placeOrderRequest)
        {
            //Tbl.Protos.PlaceOrderRequest request = _mapper.Map<Tbl.Protos.PlaceOrderRequest>(placeOrderRequest); // TODO: Is it worth it?
            var tblRequest = new Tbl.Protos.PlaceOrderRequest {Uic = placeOrderRequest.Uic};
            var retVal = await _tblClient.Place3WayOrderAsync(tblRequest);
            foreach (Tbl.Protos.OrderRequestResult result in retVal.Result)
                yield return new OrderRequestResult {IsRejected = result.IsRejected};
        }
    }
}