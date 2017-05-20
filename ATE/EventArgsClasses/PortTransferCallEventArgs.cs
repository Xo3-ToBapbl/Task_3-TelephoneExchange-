using ATE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.EventArgsClasses
{
    public class PortTransferCallEventArgs: EventArgs, ICallingEventArgs
    {
        public PortTransferCallEventArgs(int sourceNumber, int targetNumber, IPort port)
        {
            SourceNumber = sourceNumber;
            TargetNumber = targetNumber;
            Port = port;
        }

        public IPort Port { get; }
        public int SourceNumber { get; }
        public int TargetNumber { get; }
    }
}
