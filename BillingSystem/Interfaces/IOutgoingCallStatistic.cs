using System;

namespace BillingSystem.Interfaces
{
    public interface IOutgoingCallStatistic: INotCalledStatistic
    {
        int Cost { get; }
        TimeSpan Span { get; }
    }
}
