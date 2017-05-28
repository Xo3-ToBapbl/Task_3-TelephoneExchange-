using System;

namespace BillingSystem.Interfaces
{
    public interface IOutgoingCallStatistic: INotCalledStatistic
    {
        int Cost { get; set; }
        TimeSpan Span { get; }
    }
}
