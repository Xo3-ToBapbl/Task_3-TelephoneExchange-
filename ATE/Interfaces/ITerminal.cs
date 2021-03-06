﻿using ATE.Enums;
using System;

namespace ATE.Interfaces
{
    public interface ITerminal
    {
        event EventHandler Connecting;
        event EventHandler Disconnecting;
        event EventHandler<ICallingEventArgs> Calling;
        event EventHandler<ICallingEventArgs> Answering;
        event EventHandler<ICallingEventArgs> Rejecting;

        TerminalStates State { get; set; }
        int Number { get; }

        void Connect();
        void Disconect();
        void Call(int targetNumber);
        void Answer();
        void Reject();
        void SetIncommingCallState(object sender, ICallingEventArgs e);
        void SetOnlineState(object sender, ICallingEventArgs e);
        void SetConnectedState(object sender, ICallingEventArgs e);
    }
}
