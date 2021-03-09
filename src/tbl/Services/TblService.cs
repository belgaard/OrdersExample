using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Tbl.Protos;

namespace Tbl.TestDouble.Services
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Instantiated in the Startup class")]
    public class TblService : Tbl.Protos.Tbl.TblBase
    {
        private readonly ILogger<TblService> _logger;
        public TblService(ILogger<TblService> logger) => _logger = logger;

        public override Task<OrderRequestResults> Place3WayOrder(PlaceOrderRequest placeOrderRequest,
            ServerCallContext context)
        {
            OrderRequestResults retVal = new OrderRequestResults();
            retVal.Result.Add(new OrderRequestResult {IsRejected = false, OrderId = new EntityId {Value = 42}});
            return Task.FromResult(retVal);
        }
    }
}
