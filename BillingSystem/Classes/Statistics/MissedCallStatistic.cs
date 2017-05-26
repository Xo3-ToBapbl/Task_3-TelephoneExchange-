using BillingSystem.Interfaces;
using System;

namespace BillingSystem.Classes.Statistics
{
    public class MissedCallStatistic : Statistic, IMissedCallStatistic
    {
        public MissedCallStatistic(DateTime startTime, int sourceNumber):
            base(startTime)
        {
            SourceNumber = sourceNumber;
            base.CallState = "MissedCall";
        }

        public int SourceNumber { get; }
    }
}
