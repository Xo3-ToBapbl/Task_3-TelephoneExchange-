using BillingSystem.Interfaces;
using System;

namespace BillingSystem.Classes
{
    public abstract class Statistic : IStatistic
    {
        protected string _callState;

        protected Statistic(DateTime start)
        {
            StartTime = start;
        }
 

        public string CallState
        {
            get
            {
                return _callState;
            }
            protected set
            {
                _callState = value;
            }
        }

        public DateTime StartTime { get; protected set; }
    }
}
