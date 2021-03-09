using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orders.Infrastructure.Extensions;
using Orders.Qte;
using Orders.SharedDomain;
using PlaceQteOrderRequest = Orders.Qte.PlaceOrderRequest;

namespace Orders.Orders.PlaceOrder
{
    public class PlaceOrderRequestHandler
    {
        private readonly Qte.Qte _qte;
        private readonly QteOrderMapper _qteOrderMapper;

        public PlaceOrderRequestHandler(Qte.Qte qte, QteOrderMapper qteOrderMapper)
        {
            _qte = qte;
            _qteOrderMapper = qteOrderMapper;
        }

        public async Task<GenericOrderResponse> ProcessAsync(PlaceOrderRequest request)
        {
            // Validate request
            //_requestValidator.ValidateAndThrow(request);

            // Domain validation
            //ValidationResult validationResult =
            //    await
            //        _domainValidator.ValidateAsync(ServiceRequest.Create(serviceContext, request))
            //            .ConfigureAwait(false);

            // Call QTE
            await Task.Delay(1);
            if (request.HasMasterOrder() || request.PositionId.AsNullableLong().HasValue)
                return Place3WayOrderInQuoteEngine(request);

            throw new NotSupportedException("Request is neither an OCO, or a related order to existing order");
        }

        private GenericOrderResponse Place3WayOrderInQuoteEngine(PlaceOrderRequest request)
        {
            PlaceQteOrderRequest qteOrderRequest = _qteOrderMapper.ToQuoteEnginePlaceOrderRequest(request);

            OrderRequestResult[] orderResults = _qte.PlaceOrder(qteOrderRequest).ToArray();

            return MapQteResultToResponse(request, orderResults);
        }

        private GenericOrderResponse MapQteResultToResponse(PlaceOrderRequest request, OrderRequestResult[] orderRequestResults)
        {
            // Build list of order responses
            var subOrderResponseList = new List<GenericOrderResponse.RelatedOrder>();
            for (int i = 0; i < request.GetTotalNumberOfOrders(); i++)
            {
                var subOrderResponse = new GenericOrderResponse.RelatedOrder();
                if (orderRequestResults.Length >= i + 1)
                {
                    var orderResult = orderRequestResults[i];
                    if (orderResult.IsRejected)
                        subOrderResponse.ErrorInfo = new ErrorResponse<string> {ErrorCode = "Rejected"};
                    if (orderResult.OrderId != 0) 
                        subOrderResponse.OrderId = orderResult.OrderId.ToString();
                }
                else
                {
                    var errorCode = "OrderNotPlaced";
                    var message = "Order not placed as other order in request was rejected.";
                    
                    // No result from QTE
                    subOrderResponse.ErrorInfo = new ErrorResponse<string>
                    {
                        ErrorCode = errorCode,
                        Message = message
                    };
                }

                subOrderResponseList.Add(subOrderResponse);
            }

            var response = new GenericOrderResponse();

            if (request.HasMasterOrder())
            {
                // Convert first sub order response to "master"
                response.OrderId = subOrderResponseList[0].OrderId;
                response.ErrorInfo = subOrderResponseList[0].ErrorInfo;
                subOrderResponseList.RemoveAt(0);
            }
            
            // Add rest of sub order responses
            if (subOrderResponseList.Count > 0)
            {
                response.Orders = subOrderResponseList.ToArray();
            }

            return response;
        }
    }
}