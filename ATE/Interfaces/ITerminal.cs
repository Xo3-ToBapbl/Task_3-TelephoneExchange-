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
        event EventHandler<ICallingEventArgs> Answering;
        event EventHandler<ICallingEventArgs> Ignoring;
        event EventHandler<ICallingEventArgs> Ending;

        TerminalStates State { get; set; }
        int Number { get; }

        void Connect();
        void Disconect();
        void Call(int targetNumber);
        void Answer();
        void Ignore();
        void EndCall();
        void SetIncommingCallState(object sender, ICallingEventArgs e);
        void SetOnlineState(object sender, ICallingEventArgs e);
        void SetConnectedState(object sender, ICallingEventArgs e);
    }
}
