using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading.Tasks;
using Orders.ExternalDependencies;
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
        private readonly IInstrumentCacheFacade _instrumentCache;

        public Qte(ISessionTradesFacade session, IInstrumentCacheFacade instrumentCache)
        {
            _session = session;
            _instrumentCache = instrumentCache;
        }

        /// <summary>Place an order.</summary>
        public async Task<IEnumerable<OrderRequestResult>> PlaceOrder(PlaceOrderRequest placeOrderRequest)
        {
            if (!_instrumentCache.IsInstrumentTradableByUic(placeOrderRequest.Uic))
                throw new TradeException { Reason = RejectReasons.InstrumentNotAllowed };

            OrderRequestResult[] orderRequestResults = await _session.Place3WayOrder(placeOrderRequest).ToArrayAsync();
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
        UnknownOpenOrderDuration,
        /// <summary>Working until specified date</summary>
        GoodTillDate
    }
    public class PlaceRelatedOrderRequest
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
      public long Value { get; set; }

      private EntityId(int value) => Value = (long) value;

      private EntityId(long value) => Value = value;

      public override string ToString() => Value.ToString((IFormatProvider) CultureInfo.InvariantCulture);

      public void GetObjectData(SerializationInfo info, StreamingContext context) => info?.AddValue("Value", Value);

      public override int GetHashCode() => Value.GetHashCode();

      public bool Equals(EntityId other) => Value == other.Value;

      public override bool Equals(object obj) => obj is EntityId other && Equals(other);

      public static bool operator ==(EntityId first, EntityId second) => first.Equals(second);

      public static bool operator !=(EntityId first, EntityId second) => !(first == second);

      public static implicit operator int(EntityId entityId) => (int) entityId.Value;

      public static implicit operator EntityId(int entityId) => new EntityId(entityId);

      public int ToInt32() => (int) Value;

      public EntityId FromInt32(int value) => new EntityId(value);

      public static implicit operator long(EntityId entityId) => entityId.Value;

      public static implicit operator EntityId(long entityId) => new EntityId(entityId);

      public long ToInt64() => Value;

      public EntityId FromInt64(long value) => new EntityId(value);
  }

  public readonly struct TrailingStop : IEquatable<TrailingStop>
  {
      public TrailingStop(double distanceToMarket, double step)
      {
          DistanceToMarket = distanceToMarket;
          Step = step;
      }

      public double DistanceToMarket { get; }

      public double Step { get; }

      public override int GetHashCode() => DistanceToMarket.GetHashCode() * 397 ^ Step.GetHashCode();

      public bool Equals(TrailingStop other) => DistanceToMarket.Equals(other.DistanceToMarket) && Step.Equals(other.Step);

      public override bool Equals(object obj) => obj != null && obj is TrailingStop other && Equals(other);

      public static bool operator ==(TrailingStop first, TrailingStop second) => first.Equals(second);

      public static bool operator !=(TrailingStop first, TrailingStop second) => !(first == second);
  }
  public class OrderRequestResult
  {
      public OrderRequestResult(EntityId? orderId = null) => OrderId = orderId ?? (EntityId) 0;

      public EntityId OrderId { get; }
      public bool IsRejected { get; set; }
  }
  public class QuoteEngineException : Exception
  {
      public QuoteEngineException(string message)
          : base(message)
      {
      }

      public QuoteEngineException()
          : base("Unexpected exception")
      {
      }

      public QuoteEngineException(string message, Exception innerException)
          : base(message, innerException)
      {
      }

      protected QuoteEngineException(SerializationInfo info, StreamingContext context)
          : base(info, context)
      {
      }
  }

    /// <summary>Reject reasons</summary>
  public enum RejectReasons
  {
    /// <summary>None (-1)</summary>
    [Description("None (-1)")] None = -1, // 0xFFFFFFFF
    /// <summary>"All Ok"</summary>
    [Description("All Ok")] Accepted = 0,
    /// <summary>"Illegal amount"</summary>
    [Description("Illegal amount")] IllegalAmount = 3,
    /// <summary>"Not allowed to trade in this instrument"</summary>
    [Description("Not allowed to trade in this instrument")] InstrumentNotAllowed = 6,
  }

  public class TradeException : QuoteEngineException
  {
      public RejectReasons Reason { get; set; }

      public bool SkipRejectionLog { get; set; }

      public TradeException(string message, RejectReasons errorCode, bool skipRejectionLog = false)
          : base(string.Format("{0} ({1})", (object) message, (object) errorCode))
      {
          this.Reason = errorCode;
          this.SkipRejectionLog = skipRejectionLog;
      }

      public TradeException(
          string message,
          Exception innerException,
          RejectReasons reason,
          bool skipRejectionLog = false)
          : base(message, innerException)
      {
          this.Reason = reason;
          this.SkipRejectionLog = skipRejectionLog;
      }

      public TradeException(string message)
          : base(message)
      {
      }

      public TradeException()
      {
      }

      public TradeException(string message, Exception innerException)
          : base(message, innerException)
      {
      }

      [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
      public override void GetObjectData(SerializationInfo info, StreamingContext context) => base.GetObjectData(info, context);
  }}