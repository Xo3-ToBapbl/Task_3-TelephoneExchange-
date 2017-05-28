using System;

namespace BillingSystem.Interfaces
{
    public interface IIncomingCallStatistic: IMissedCallStatistic
    {
        TimeSpan Span { get; }
    }
}
