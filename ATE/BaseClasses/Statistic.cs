using ATE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.BaseClasses
{
    public class Statistic
    {
        public int Number { get; }

        public CallStats CallState { get; }

        public int TargetNumber { get; }

        public DateTime Start { get; }

        public TimeSpan Span { get; }

        public int Cost { get; }
    }
}
