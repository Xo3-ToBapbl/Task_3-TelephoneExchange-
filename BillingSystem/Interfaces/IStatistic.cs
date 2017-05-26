using System;

namespace BillingSystem.Interfaces
{
    public interface IStatistic
    {
        string CallState { get; }
        DateTime StartTime { get; }
    }
}
