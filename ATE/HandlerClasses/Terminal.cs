using ATE.Enums;
using ATE.EventArgsClasses;
using ATE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.HandlerClasses
{
    public class Terminal : ITerminal
    {
        public Terminal(int number)
        {
            _number = number;
        }

        private int _number;
        private TerminalStates _state;
        private ICallingEventArgs _e;

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
        public event EventHandler<ICallingEventArgs> Ignoring;
        public event EventHandler<ICallingEventArgs> Ending;

        #region Start event methods:
        protected virtual void OnConnecting(object sender, EventArgs args)
        {
            Connecting?.Invoke(sender, args);
        }

        protected virtual void OnDisconnecting(object sender, EventArgs args)
        {
            Disconnecting?.Invoke(sender, args);
        }

        protected virtual void OnCalling(int targetNumber)
        {
            Calling?.Invoke(this, new CallEventArgs(this.Number, targetNumber));
        }

        protected virtual void OnAnswering()
        {
            Answering?.Invoke(this, _e);
        }

        protected virtual void OnIgnoring()
        {
            Ignoring?.Invoke(this, _e);
        }

        protected virtual void OnEnding()
        {
            Ending?.Invoke(this, _e);
        }

        #endregion

        public void Connect()
        {
            State = TerminalStates.Connected;
            OnConnecting(this, null);
        }   

        public void Disconect()
        {
            State = TerminalStates.Disconnected;
            OnDisconnecting(this, null);
        }

        public void Call(int targetNumber)
        {
            if (State == TerminalStates.Connected)
            {
                State = TerminalStates.OutgoingCall;
                OnCalling(targetNumber);
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
        }

        public void SetOnlineState(object sender, ICallingEventArgs e)
        {
            if (State == TerminalStates.OutgoingCall)
            {
                _e = e;
                State = TerminalStates.Online;
                Console.WriteLine("Terminal {0}: online with terminal {1}.\n", this.Number, e.TargetNumber);
            }
        }

        public void Ignore()
        {
            if (State == TerminalStates.IncommingCall)
            {
                Console.WriteLine("Terminal {0}: ignore a call from terminal {1}.\n", this.Number, _e.SourceNumber);

                State = TerminalStates.Connected;
                OnIgnoring();
            }
        }

        public void SetConnectedState(object sender, ICallingEventArgs e)
        {
            if (State == TerminalStates.OutgoingCall || State == TerminalStates.Online)
            {
                State = TerminalStates.Connected;
                Console.WriteLine("Terminal {0}: terminal {1} ignore or end call.\n", this.Number, e.TargetNumber);
            }
        }

        public void EndCall()
        {
            if (State == TerminalStates.Online)
            {
                Console.WriteLine("Terminal {0}: end call with terminal {1}.\n", this.Number, _e.SourceNumber);

                State = TerminalStates.Connected;
                OnEnding();
            }
        }
    }
}
