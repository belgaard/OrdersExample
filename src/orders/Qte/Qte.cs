﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Orders.ExternalDependencies;
using Orders.Orders;
using Orders.Orders.PlaceOrder;
using Orders.SharedDomain;

namespace Orders.Qte
{
    public static class OrderMapping
    {
        /// <summary>Maps PlaceableOrderType to the external <see cref="OpenOrderType"/>.</summary>
        public static OpenOrderType MapOrderType(PlaceableOrderType? orderType)
        {
            switch (orderType.GetValueOrDefault())
            {
                case PlaceableOrderType.Market:
                    return OpenOrderType.Market;
                case PlaceableOrderType.Limit:
                    return OpenOrderType.Limit;
                case PlaceableOrderType.TrailingStop:
                    return OpenOrderType.Stop;
                default:
                    throw new ArgumentOutOfRangeException("Cannot map orderType '" + orderType.GetValueOrDefault() + "' to FrontOffice.Trading.Public.OpenOrderType.");
            }
        }
        /// <summary>Maps our OrderDuration to external <see cref="OpenOrderDuration"/>.</summary>
        public static OpenOrderDuration? MapOrderDuration(OrderDuration orderDuration)
        {
            if (orderDuration == null) throw new ArgumentNullException(nameof(orderDuration));

            switch (orderDuration.DurationType)
            {
                case OrderDurationType.GoodTillDate:
                    return OpenOrderDuration.GoodTillDate;
                default:
                    throw new ArgumentOutOfRangeException("Cannot map orderDuration.DurationType '" + orderDuration.DurationType + "' to FrontOffice.Trading.Public.OpenOrderDuration");
            }
        }
    }

    public class Qte
    {
        private readonly ISessionTradesFacade _session;

        public Qte(ISessionTradesFacade session) => _session = session;

        /// <summary>Place an order.</summary>
        public IEnumerable<OrderRequestResult> PlaceOrder(PlaceOrderRequest placeOrderRequest)
        {
            IEnumerable<OrderRequestResult> result = _session.Place3WayOrder(placeOrderRequest);

            OrderRequestResult[] orderRequestResults = result as OrderRequestResult[] ?? result.ToArray();
            if (orderRequestResults.Length > 0)
            {
                // Handle logging ...
            }
            return orderRequestResults;
        }
    }
    /// <summary>Order durations</summary>
    public enum OpenOrderDuration
    {
        /// <summary>Unspecified duration</summary>
        Unknown,
        /// <summary>Working until specified date</summary>
        GoodTillDate
    }
    public struct PlaceRelatedOrderRequest
    {
        public BuySell BuySell { get; set; }

        public double Amount { get; set; }

        public double Price { get; set; }

        public OpenOrderType OrderType { get; set; }

        public OpenOrderDuration OrderDuration { get; set; }

        public OrderExpireData? OrderExpireData { get; set; }

        public TrailingStop? TrailingStop { get; set; }

        public OpenOrderRelation? OpenOrderRelation { get; set; }
        public double? OrderPriceLevel2 { get; set; }
    }
    public struct OrderExpireData
    {
        public OrderExpireData(DateTime expireDateTime) => ExpireDateTime = expireDateTime;

        public DateTime ExpireDateTime { get; }
    }
/// <summary>Instrument types</summary>
  [Flags]
  public enum InstrumentTypes
  {
    /// <summary> None </summary>
    None = 0,
    /// <summary> Shares </summary>
    Shares = 32, // 0x00000020
    /// <summary> Cfd </summary>
    Cfd = 128, // 0x00000080
 }
  public struct PlaceOrderRequest
  {
    public int Uic { get; set; }

    public InstrumentTypes InstrumentType { get; set; }

    public BuySell BuySell { get; set; }

    public double Amount { get; set; }

    public double Price { get; set; }

    public OpenOrderType OrderType { get; set; }

    public OpenOrderDuration OrderDuration { get; set; }

    public OrderExpireData? OrderExpireData { get; set; }
    public double? OrderPriceLevel2 { get; set; }
    public TrailingStop? TrailingStop { get; set; }
    public OpenOrderRelation? OpenOrderRelation { get; set; }
    public EntityId? RelatedPositionId { get; set; }
    public IEnumerable<PlaceRelatedOrderRequest> RelatedOrders { get; set; }
  }
  [Serializable]
  public struct EntityId : IEquatable<EntityId>, ISerializable
  {
      public static readonly EntityId Default = new EntityId(0);
      private readonly long _value;

      private EntityId(int value) => this._value = (long) value;

      private EntityId(long value) => this._value = value;

      public override string ToString() => this._value.ToString((IFormatProvider) CultureInfo.InvariantCulture);

      public void GetObjectData(SerializationInfo info, StreamingContext context) => info?.AddValue("Value", this._value);

      public override int GetHashCode() => this._value.GetHashCode();

      public bool Equals(EntityId other) => this._value == other._value;

      public override bool Equals(object obj) => obj is EntityId other && this.Equals(other);

      public static bool operator ==(EntityId first, EntityId second) => first.Equals(second);

      public static bool operator !=(EntityId first, EntityId second) => !(first == second);

      public static implicit operator int(EntityId entityId) => (int) entityId._value;

      public static implicit operator EntityId(int entityId) => new EntityId(entityId);

      public int ToInt32() => (int) this._value;

      public EntityId FromInt32(int value) => new EntityId(value);

      public static implicit operator long(EntityId entityId) => entityId._value;

      public static implicit operator EntityId(long entityId) => new EntityId(entityId);

      public long ToInt64() => this._value;

      public EntityId FromInt64(long value) => new EntityId(value);
  }

  public readonly struct TrailingStop : IEquatable<TrailingStop>
  {
      public TrailingStop(Decimal distanceToMarket, Decimal step)
      {
          this.DistanceToMarket = distanceToMarket;
          this.Step = step;
      }

      public Decimal DistanceToMarket { get; }

      public Decimal Step { get; }

      public override int GetHashCode() => this.DistanceToMarket.GetHashCode() * 397 ^ this.Step.GetHashCode();

      public bool Equals(TrailingStop other) => this.DistanceToMarket.Equals(other.DistanceToMarket) && this.Step.Equals(other.Step);

      public override bool Equals(object obj) => obj != null && obj is TrailingStop other && this.Equals(other);

      public static bool operator ==(TrailingStop first, TrailingStop second) => first.Equals(second);

      public static bool operator !=(TrailingStop first, TrailingStop second) => !(first == second);
  }
  public class OrderRequestResult
  {
      public OrderRequestResult(EntityId? orderId = null) => OrderId = orderId ?? (EntityId) 0;

      public EntityId OrderId { get; }
      public bool IsRejected { get; set; }
  }
}