using BillingSystem.Interfaces;
using System;

namespace BillingSystem.Classes
{
    public class OutgoingCallStatistic: Statistic, IOutgoingCallStatistic
    {
        public OutgoingCallStatistic(DateTime startTime, TimeSpan span, int targetNumber):
            base(startTime)
        {
            Span = span;
            TargetNumber = targetNumber;
            base.CallState = "OutgoingCall";
        }

        public int Cost { get; }

        public TimeSpan Span { get; }

        public int TargetNumber { get; }

    }
}
