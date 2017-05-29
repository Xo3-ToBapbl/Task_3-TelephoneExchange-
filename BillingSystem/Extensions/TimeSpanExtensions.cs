using System;

namespace BillingSystem.Extensions
{
    public static class TimeSpanExtensions
    {
        static public bool IsBetween(this TimeSpan time, TimeSpan startTime, TimeSpan endTime)
        {
            if (endTime == startTime)
            {
                return false;
            }
            if (endTime < startTime)
            {
                return time <= endTime || time >= startTime;
            }
            return time >= startTime && time <= endTime;
        }
    }
}
