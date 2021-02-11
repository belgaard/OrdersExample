﻿using System.Collections.Generic;
using Orders.Qte;

namespace Orders.ExternalDependencies
{
    public interface ISessionTradesFacade
    {
        IEnumerable<OrderRequestResult> Place3WayOrder(PlaceOrderRequest placeOrderRequest);
    }
    internal class SessionTradesFacade : ISessionTradesFacade
    {
        private readonly ISessionTrading _sessionTrading;
      
        public SessionTradesFacade(ISessionTrading sessionTrading)
        {
            _sessionTrading = sessionTrading;
        }

        public IEnumerable<OrderRequestResult> Place3WayOrder(PlaceOrderRequest placeOrderRequest) => _sessionTrading.Place3WayOrder(placeOrderRequest);
    }
}