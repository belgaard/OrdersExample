using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Orders.ExternalDependencies;
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
            try
            {
                // TODO: Validate request (simple input validation)!

                // Domain validation
                //ValidationResult validationResult =
                //    await
                //        _domainValidator.ValidateAsync(ServiceRequest.Create(serviceContext, request))
                //            .ConfigureAwait(false);

                // Call QTE
                await Task.Delay(1);
                if (request.HasMasterOrder() || request.PositionId.AsNullableLong().HasValue)
                    return await Place3WayOrderInQuoteEngine(request);

                throw new NotSupportedException("Request is neither an OCO, or a related order to existing order");
            }
            catch (TradeException ex)
            {
                // Build error response from RejectReason:
                return ex.Reason switch
                {
                    RejectReasons.InstrumentNotAllowed => new GenericOrderResponse
                    {
                        ErrorInfo = new ErrorResponse<string>
                        {
                            ErrorCode = "The logged-in user is not allowed to trade the instrument."
                        }
                    },
                    RejectReasons.None => throw new NotImplementedException(),
                    RejectReasons.Accepted => throw new NotImplementedException(),
                    RejectReasons.IllegalAmount => throw new NotImplementedException(),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            catch (Exception)
            {
                // Build error response from RejectReason
                return new GenericOrderResponse {ErrorInfo = new ErrorResponse<string> {ErrorCode = "An unexpected error occurred"}};
            }
        }

        private async Task<GenericOrderResponse> Place3WayOrderInQuoteEngine(PlaceOrderRequest request)
        {
            PlaceQteOrderRequest qteOrderRequest = _qteOrderMapper.ToQuoteEnginePlaceOrderRequest(request);

            OrderRequestResult[] orderResults = (await _qte.PlaceOrder(qteOrderRequest)).ToArray();

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