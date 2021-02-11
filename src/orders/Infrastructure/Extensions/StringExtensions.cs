namespace Orders.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        /// <summary>Checks whether given string is NullOrEmpty and contains valid nullable long value.</summary>
        public static long? AsNullableLong(this string @this)
        {
            if (!string.IsNullOrEmpty(@this) && long.TryParse(@this, out long longValue))
                return longValue;
            return null;
        }
    }
}