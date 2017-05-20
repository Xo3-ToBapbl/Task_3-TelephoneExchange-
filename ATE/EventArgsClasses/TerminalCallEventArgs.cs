using ATE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.EventArgsClasses
{
    public class CallEventArgs:EventArgs, ICallingEventArgs
    {
        public CallEventArgs(int sourceNumber, int targetNumber)
        {
            SourceNumber = sourceNumber;
            TargetNumber = targetNumber;
        }

        public int SourceNumber { get; }
        public int TargetNumber { get; }
    }
}
