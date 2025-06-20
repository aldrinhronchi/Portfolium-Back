namespace Portfolium_Back.Extensions.Helpers
{
    public class TimeZoneManager
    {
        private static readonly TimeZoneInfo TimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

        public static DateTime GetTime(DateTime utcDateTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, TimeZone);
        }

        public static DateTime GetTimeNow()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZone);
        }
    }
} 