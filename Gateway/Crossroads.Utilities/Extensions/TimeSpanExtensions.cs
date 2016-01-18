using System;

namespace Crossroads.Utilities.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string FormatAsString(this TimeSpan? timeSpan)
        {
            if (timeSpan == null)
            {
                return null;
            }
            var t = (TimeSpan) timeSpan;
            return DateTime.Today.Add(t).ToString("hh:mm tt");
        }

        public static string FormatAsString(this TimeSpan timeSpan)
        {
            return DateTime.Today.Add(timeSpan).ToString("hh:mm tt");
        }
    }
}
