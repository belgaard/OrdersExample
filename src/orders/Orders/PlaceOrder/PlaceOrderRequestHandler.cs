using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private BasicOrderRequestValidator _validator;

        public PlaceOrderRequestHandler(Qte.Qte qte, QteOrderMapper qteOrderMapper)
        {
            _qte = qte;
            _qteOrderMapper = qteOrderMapper;
            
            _validator = new BasicOrderRequestValidator();
        }

        public async Task<GenericOrderResponse> ProcessAsync(PlaceOrderRequest request)
        {
            try
            {
                // Validate request:
                bool isOk = false;
                StringBuilder errorInfo = new();
                if (request.HasMasterOrder())
                {
                    isOk = _validator.HasOrderTypeSpecificRequiredFields(request, request.OrderType, errorInfo);
                    if (isOk)
                        isOk = _validator.AreOrderTypeSpecificRequiredFieldsOk(request, request.OrderType, errorInfo);
                }
                else
                    if (request.HasRelatedOrders())
                    {
                        var relatedOrderRequest = request.Orders.First();
                        isOk = _validator.HasOrderTypeSpecificRequiredFields(relatedOrderRequest, relatedOrderRequest.OrderType, errorInfo);
                        if (isOk)
                            isOk = _validator.AreOrderTypeSpecificRequiredFieldsOk(relatedOrderRequest, relatedOrderRequest.OrderType, errorInfo);
                    }

                if (!isOk)
                    throw new ArgumentException(errorInfo.ToString());

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
            catch (Exception ex)
            {
                // Build error response from RejectReason
                return new GenericOrderResponse {ErrorInfo = new ErrorResponse<string> {ErrorCode = $"An unexpected error occurred: '{ex.Message}'"}};
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

    public class BasicOrderRequestValidator
    {
        public bool AreOrderTypeSpecificRequiredFieldsOk(IOrderRequest orderRequest, PlaceableOrderType? orderType, StringBuilder context)
        {
            var invalidFields = new List<string>();

            switch (orderType)
            {
                case PlaceableOrderType.Market:
                case PlaceableOrderType.Limit:
                {
                    break;
                }
                case PlaceableOrderType.TrailingStop:
                {
                    if (orderRequest.TrailingStopDistanceToMarket <= 0)
                        invalidFields.Add(nameof(IOrderRequest.TrailingStopDistanceToMarket));
                    if (orderRequest.TrailingStopStep <= 0)
                        invalidFields.Add(nameof(IOrderRequest.TrailingStopStep));
                    break;
                }
            }

            if (invalidFields.Count > 0)
            {
                context.Append($"InvalidRequiredFields: {string.Join(", ", invalidFields)}");
                return false;
            }
            return true;
        }
         public bool HasOrderTypeSpecificRequiredFields(IOrderRequest orderRequest, PlaceableOrderType? orderType, StringBuilder context)
        {
            var missingFields = new List<string>();

            switch (orderType)
            {
                case PlaceableOrderType.Market:
                case PlaceableOrderType.Limit:
                {
                    break;
                }
                case PlaceableOrderType.TrailingStop:
                {
                    if (orderRequest.TrailingStopDistanceToMarket == null)
                        missingFields.Add(nameof(IOrderRequest.TrailingStopDistanceToMarket));
                    if (orderRequest.TrailingStopStep == null)
                        missingFields.Add(nameof(IOrderRequest.TrailingStopStep));
                    break;
                }
            }

            if (missingFields.Count > 0)
            {
                context.Append($"MissingRequiredFields: {string.Join(", ", missingFields)}");
                return false;
            }
            return true;
        }
    }
}