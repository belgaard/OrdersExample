using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Orders.ExternalDependencies;
using Orders.Orders;
using Orders.Orders.PlaceOrder;
using Orders.SharedDomain;
using ProtoBuf;

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
    [ProtoContract()]
    public class PlaceRelatedOrderRequest
    {
        [ProtoMember(1)]
        public BuySell BuySell { get; set; }

        [ProtoMember(2)]
        public double Amount { get; set; }

        [ProtoMember(3)]
        public double Price { get; set; }

        [ProtoMember(4)]
        public OpenOrderType OrderType { get; set; }

        [ProtoMember(5)]
        public OpenOrderDuration OrderDuration { get; set; }

        [ProtoMember(6)]
        public OrderExpireData? OrderExpireData { get; set; }

        [ProtoMember(7)]
        public TrailingStop? TrailingStop { get; set; }

        [ProtoMember(8)]
        public OpenOrderRelation? OpenOrderRelation { get; set; }
        [ProtoMember(9)]
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
[ProtoContract]
public struct PlaceOrderRequest
  {
      [ProtoMember(1)]
      public int Uic { get; set; }

      [ProtoMember(2)]
    public InstrumentTypes InstrumentType { get; set; }

    [ProtoMember(3)]
    public BuySell BuySell { get; set; }

    [ProtoMember(4)]
    public double Amount { get; set; }

    [ProtoMember(5)]
    public double Price { get; set; }

    [ProtoMember(6)]
    public OpenOrderType OrderType { get; set; }

    [ProtoMember(7)]
    public OpenOrderDuration OrderDuration { get; set; }

    [ProtoMember(8)]
    public OrderExpireData? OrderExpireData { get; set; }
    [ProtoMember(9)]
    public double? OrderPriceLevel2 { get; set; }
    [ProtoMember(10)]
    public TrailingStop? TrailingStop { get; set; }
    [ProtoMember(11)]
    public OpenOrderRelation? OpenOrderRelation { get; set; }
    [ProtoMember(12)]
    public EntityId? RelatedPositionId { get; set; }
    [ProtoMember(13)]
    public IEnumerable<PlaceRelatedOrderRequest> RelatedOrders { get; set; }
  }
  [Serializable]
  [ProtoContract]
  public struct EntityId : IEquatable<EntityId>, ISerializable
  {
      public static readonly EntityId Default = new EntityId(0);
      [ProtoMember(1)]
      public long Value { get; set; }

      private EntityId(int value) => this.Value = (long) value;

      private EntityId(long value) => this.Value = value;

      public override string ToString() => this.Value.ToString((IFormatProvider) CultureInfo.InvariantCulture);

      public void GetObjectData(SerializationInfo info, StreamingContext context) => info?.AddValue("Value", this.Value);

      public override int GetHashCode() => this.Value.GetHashCode();

      public bool Equals(EntityId other) => this.Value == other.Value;

      public override bool Equals(object obj) => obj is EntityId other && this.Equals(other);

      public static bool operator ==(EntityId first, EntityId second) => first.Equals(second);

      public static bool operator !=(EntityId first, EntityId second) => !(first == second);

      public static implicit operator int(EntityId entityId) => (int) entityId.Value;

      public static implicit operator EntityId(int entityId) => new EntityId(entityId);

      public int ToInt32() => (int) this.Value;

      public EntityId FromInt32(int value) => new EntityId(value);

      public static implicit operator long(EntityId entityId) => entityId.Value;

      public static implicit operator EntityId(long entityId) => new EntityId(entityId);

      public long ToInt64() => this.Value;

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