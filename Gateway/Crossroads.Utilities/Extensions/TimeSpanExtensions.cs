using System;

namespace Crossroads.Utilities.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string FormatAsString(this TimeSpan timeSpan)
        {
            return DateTime.Today.Add(timeSpan).ToString("hh:mm tt");
        }
    }
}
