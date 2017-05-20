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
        private States _state;

        public int Number
        {
            get
            {
                return _number;
            }
        }

        public States State
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
        #endregion

        public void Connect()
        {
            State = States.Free;
            OnConnecting(this, null);
        }   

        public void Disconect()
        {
            State = States.Disabled;
            OnDisconnecting(this, null);
        }

        public void Call(int targetNumber)
        {
            if (State == States.Free)
            {
                State = States.Busy;
                OnCalling(targetNumber);
            }
        }
    }
}
