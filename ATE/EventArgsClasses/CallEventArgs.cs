using ATE.Interfaces;
using System;

namespace ATE.EventArgsClasses
{
    public class CallEventArgs: EventArgs, ICallingEventArgs
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
