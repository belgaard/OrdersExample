using System.Threading.Tasks;

namespace Orders.Orders.PlaceOrder
{
    public class PlaceOrderRequestHandler
    {
        public async Task<OrderResponse> ProcessAsync(PlaceOrderRequest request)
        {
            await Task.Delay(1);
            return new OrderResponse {OrderId = "42"};
        }
    }
}