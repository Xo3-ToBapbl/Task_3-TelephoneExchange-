using BillingSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Classes
{
    public class Statistic : IStatistic
    {
        public Statistic(int sourceNumber, int targetNumber, 
            string callstate, DateTime start, TimeSpan span)
        {
            Number = sourceNumber;
            TargetNumber = targetNumber;
            CallState = callstate;
            Start = start;
            Span = span;
        }


        public int Number { get; }

        public int TargetNumber { get; }

        public string CallState { get; }

        public DateTime Start { get; }

        public TimeSpan Span { get; }

        public int Cost { get; }
    }
}
