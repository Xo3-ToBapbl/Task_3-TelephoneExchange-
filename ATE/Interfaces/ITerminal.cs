using ATE.Enums;
using ATE.EventArgsClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.Interfaces
{
    public interface ITerminal
    {
        event EventHandler Connecting;
        event EventHandler Disconnecting;
        event EventHandler<ICallingEventArgs> Calling;

        States State { get; set; }

        void Connect();
        void Disconect();
        void Call(int targetNumber);
    }
}
