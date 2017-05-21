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

        private PortStates _state = PortStates.Disabled;

        public PortStates State
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

        public event EventHandler<PortStates> PortStateChanging;
        public event EventHandler<ICallingEventArgs> PortCallSending;
        public event EventHandler<ICallingEventArgs> PortCallReciving;
        public event EventHandler<ICallingEventArgs> PortAnswerSending;
        public event EventHandler<ICallingEventArgs> PortAnswerReciving;


        #region Start event methods:
        protected virtual void OnPortStateChanged(object sender, PortStates state)
        {
            PortStateChanging?.Invoke(sender, state);
        }

        protected virtual void OnPortCallSend(object sender, ICallingEventArgs e)
        {
            Console.WriteLine("Port: terminal {0} calling", e.SourceNumber);

            this.State = PortStates.Busy;
            PortCallSending?.Invoke(this, e);
        }

        protected virtual void OnPortCallRecive(object sender, ICallingEventArgs e)
        {
            Console.WriteLine("Port: recive call from {0} to {1}", e.SourceNumber, e.TargetNumber);

            PortCallReciving?.Invoke(sender, e);
        }

        protected virtual void OnPortAnswerSend(object sender, ICallingEventArgs e)
        {
            Console.WriteLine("Port: transfer answer from {0} to {1}", e.TargetNumber, e.SourceNumber);

            PortAnswerSending?.Invoke(this, e);
        }

        protected virtual void OnPortAnswerRecive(object sender, ICallingEventArgs e)
        {
            Console.WriteLine("Port: recive answer from {0} to {1}", e.TargetNumber, e.SourceNumber);

            PortAnswerReciving?.Invoke(sender, e);
        }
        #endregion

        private void SetStateFree(object obj, EventArgs args)
        {
            this.State = PortStates.Free;
            _terminal.Calling += OnPortCallSend;
            _terminal.Answering += OnPortAnswerSend;
            this.PortCallReciving += _terminal.SetIncommingCallState;
            this.PortAnswerReciving += _terminal.SetOnlineState;
        }

        private void SetStateDisabled(object obj, EventArgs args)
        {
            this.State = PortStates.Disabled;
            _terminal.Calling -= OnPortCallSend;
            _terminal.Answering -= OnPortAnswerSend;
            this.PortCallReciving -= _terminal.SetIncommingCallState;
            this.PortAnswerReciving -= _terminal.SetOnlineState;

        }

        public void PortReciveCall(object sender, ICallingEventArgs e)
        {
            this.State = PortStates.Busy;
            OnPortCallRecive(sender, e);
        }

        public void PortReciveAnswer(object sender, ICallingEventArgs e)
        {
            OnPortAnswerRecive(sender, e);
        }
    }
}
