using BillingSystem.Interfaces;
using System;

namespace BillingSystem.Classes.Statistics
{
    public class IncomingCallStatistic: Statistic, IIncomingCallStatistic
    {
        public IncomingCallStatistic(DateTime startTime, TimeSpan span, int sourceNumber):
            base(startTime)
        {
            Span = span;
            SourceNumber = sourceNumber;
            base.CallState = "IncomingCall";
        }

        public int Cost { get; }

        public TimeSpan Span { get; }

        public int SourceNumber { get; }
    }
}
