using System;

namespace BillingSystem.Interfaces
{
    public interface IIncomingCallStatistic: IMissedCallStatistic
    {
        int Cost { get; }
        TimeSpan Span { get; }
    }
}
