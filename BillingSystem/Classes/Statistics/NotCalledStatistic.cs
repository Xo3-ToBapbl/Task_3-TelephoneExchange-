using BillingSystem.Interfaces;
using System;

namespace BillingSystem.Classes.Statistics
{
    public class NotCalledStatistic : Statistic, INotCalledStatistic
    {
        public NotCalledStatistic(DateTime startTime, int tergetNumber):
            base(startTime)
        {
            TargetNumber = tergetNumber;
            base.CallState = "NotCalled";
        }

        public int TargetNumber { get; }
    }
}
