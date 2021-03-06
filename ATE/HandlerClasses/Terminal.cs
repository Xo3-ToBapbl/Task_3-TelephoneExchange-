﻿using ATE.Enums;
using ATE.EventArgsClasses;
using ATE.Interfaces;
using System;

namespace ATE.HandlerClasses
{
    public class Terminal : ITerminal
    {
        private int _number;
        private TerminalStates _state;
        private ICallingEventArgs _e;

        public Terminal(int number)
        {
            _number = number;
        }
      
        public int Number
        {
            get
            {
                return _number;
            }
        }

        public TerminalStates State
        {
            get
            {
                return _state;
            }
            set
            {
                if (value != _state)
                {
                    _state = value;
                }
            }
        }

        public event EventHandler Connecting;
        public event EventHandler Disconnecting;
        public event EventHandler<ICallingEventArgs> Calling;
        public event EventHandler<ICallingEventArgs> Answering;
        public event EventHandler<ICallingEventArgs> Rejecting;

        #region Start event methods:

        protected virtual void OnConnecting(object sender, EventArgs args)
        {
            Console.WriteLine("Terminal {0}: connected.\n", this.Number);

            Connecting?.Invoke(sender, args);
        }

        protected virtual void OnDisconnecting(object sender, EventArgs args)
        {
            Console.WriteLine("\nTerminal {0}: disconnected.\n", this.Number);

            Disconnecting?.Invoke(sender, args);
        }

        protected virtual void OnCalling(int targetNumber)
        {
            _e = new CallEventArgs(this.Number, targetNumber);
            Calling?.Invoke(this, _e);
        }

        protected virtual void OnAnswering()
        {
            Answering?.Invoke(this, _e);
        }

        protected virtual void OnRejecting()
        {
            Rejecting?.Invoke(this, _e);
        }

        #endregion

        public void Connect()
        {
            if (Connecting != null)
            {
                State = TerminalStates.Connected;
                OnConnecting(this, null);
            }
        }   

        public void Disconect()
        {
            State = TerminalStates.Disconnected;
            OnDisconnecting(this, null);
        }

        public void Call(int targetNumber)
        {
            Console.WriteLine("\n");
            if (State == TerminalStates.Connected)
            {
                if (targetNumber != this.Number)
                {
                    State = TerminalStates.OutgoingCall;
                    OnCalling(targetNumber);
                }
                else
                {
                    Console.WriteLine("Terminal {0}: You are trying to call yourself!", this.Number);
                }
            }
            else
            {
                Console.WriteLine("Terminal {0}: please, connect me before calling!", this.Number);
            }
        }

        public void SetIncommingCallState(object sender, ICallingEventArgs e)
        {
            Console.WriteLine("Terminal {0}: have incomming call from terminal {1}.\n", e.TargetNumber, e.SourceNumber);

            if (State == TerminalStates.Connected)
            {
                State = TerminalStates.IncommingCall;
                _e = e;
            }
        }

        public void Answer()
        {
            if (State == TerminalStates.IncommingCall)
            {
                Console.WriteLine("Terminal {0}: answer a call from terminal {1}.\n", this.Number, _e.SourceNumber);

                State = TerminalStates.Online;
                OnAnswering();
            }
            else
            {
                Console.WriteLine("Terminal {0}: You need to call before you can answer the call.\n", this.Number);
            }
        }

        public void SetOnlineState(object sender, ICallingEventArgs e)
        {
            if (State == TerminalStates.OutgoingCall)
            {
                State = TerminalStates.Online;
                Console.WriteLine("Terminal {0}: online with terminal {1}.\n", this.Number, e.TargetNumber);
            }
        }

        public void SetConnectedState(object sender, ICallingEventArgs e)
        {
            if (State == TerminalStates.OutgoingCall || State == TerminalStates.Online)
            {
                Console.WriteLine("Terminal {0}: call rejected.\n", this.Number);

                State = TerminalStates.Connected;
            }
        }

        public void Reject()
        {
            if (State == TerminalStates.Online || State == TerminalStates.IncommingCall)
            {
                Console.WriteLine("Terminal {0}: reject call.\n", this.Number);

                State = TerminalStates.Connected;
                OnRejecting();
            }
        }
    }
}
