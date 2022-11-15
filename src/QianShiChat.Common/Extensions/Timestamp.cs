namespace QianShiChat.Common.Extensions
{
    public static class Timestamp
    {
        public static long ToTimestamp(this DateTimeOffset time)
        {
            return time.ToUnixTimeMilliseconds();
        }

        public static DateTimeOffset ToDateTimeOffset(this long timestamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(timestamp);
        }

        public static long Now => DateTimeOffset.Now.ToTimestamp();
    }
}
