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
    public class Port: IPort
    {
        private ITerminal _terminal;

        public Port(ITerminal terminal)
        {
            _terminal = terminal;
            _terminal.Connecting += this.SetStateFree;
            _terminal.Disconnecting += this.SetStateDisabled;
        }

        private States _state = States.Disabled;

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
                    OnPortStateChanged(this, _state);
                }
            }
        }

        public event EventHandler<States> PortStateChanging;
        public event EventHandler<ICallingEventArgs> PortCallTransfering;

        #region Start event methods:
        protected virtual void OnPortStateChanged(object sender, States state)
        {
            PortStateChanging?.Invoke(sender, state);
        }

        protected virtual void OnPortCallTransfer(object sender, ICallingEventArgs e)
        {
            Console.WriteLine("Port: terminal {0} calling", e.SourceNumber);
            PortCallTransfering?.Invoke(sender, e);
        }
        #endregion

        private void SetStateFree(object obj, EventArgs args)
        {
            this.State = States.Free;
            _terminal.Calling += OnPortCallTransfer;
        }

        private void SetStateDisabled(object obj, EventArgs args)
        {
            this.State = States.Disabled;
            _terminal.Calling -= OnPortCallTransfer;
        }
    }
}
