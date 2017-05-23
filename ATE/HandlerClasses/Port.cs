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
        private string _portId;
        private PortStates _state = PortStates.Disabled;

        public Port(ITerminal terminal)
        {
            _terminal = terminal;
            _portId = string.Format("p{0}", _terminal.Number.ToString());

            _terminal.Connecting += this.SetStateFree;
            _terminal.Disconnecting += this.SetStateDisabled;
        }

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

        public string PortId
        {
            get
            {
                return _portId;
            }
        }

        public event EventHandler<PortStates> PortStateChanging;
        public event EventHandler<ICallingEventArgs> PortCallSending;
        public event EventHandler<ICallingEventArgs> PortCallReciving;
        public event EventHandler<ICallingEventArgs> PortAnswerSending;
        public event EventHandler<ICallingEventArgs> PortAnswerReciving;
        public event EventHandler<ICallingEventArgs> PortRejectSending;
        public event EventHandler<ICallingEventArgs> PortRejectReciving;


        #region Start event methods:

        protected virtual void OnPortStateChanged(object sender, PortStates state)
        { 
            //Console.WriteLine("Port[{0}]: changing state to '{1}', terminal {2} now {3}.\n", 
                //this.PortId, state, _terminal.Number, _terminal.State);

            PortStateChanging?.Invoke(sender, state);
        }

        protected virtual void OnPortCallSend(object sender, ICallingEventArgs e)
        {
            Console.WriteLine("Port[{0}]: terminal {1} calling to terminal {2}.\n", 
                this.PortId, e.SourceNumber, e.TargetNumber);

            this.State = PortStates.Busy;
            PortCallSending?.Invoke(this, e);
        }

        protected virtual void OnPortCallRecive(object sender, ICallingEventArgs e)
        {
            Console.WriteLine("Port[{0}]: recive call from {1} to {2}.\n",
                this.PortId, e.SourceNumber, e.TargetNumber);

            PortCallReciving?.Invoke(sender, e);
        }

        protected virtual void OnPortAnswerSend(object sender, ICallingEventArgs e)
        {
            Console.WriteLine("Port[{0}]: transfer answer from {1} to {2}.\n",
                this.PortId, e.TargetNumber, e.SourceNumber);

            PortAnswerSending?.Invoke(this, e);
        }

        protected virtual void OnPortAnswerRecive(object sender, ICallingEventArgs e)
        {
            Console.WriteLine("Port[{0}]: recive answer from {1} to {2}.\n",
                this.PortId, e.TargetNumber, e.SourceNumber);

            PortAnswerReciving?.Invoke(sender, e);
        }

        protected virtual void OnPortRejectSend(object sender, ICallingEventArgs e)
        {
            Console.WriteLine("Port[{0}]: transfer reject.\n",
                this.PortId);

            this.State = PortStates.Free;
            PortRejectSending?.Invoke(this, e);
        }

        protected virtual void OnPortRejectRecive(object sender, ICallingEventArgs e)
        {
            Console.WriteLine("Port[{0}]: recive reject.\n", this.PortId);

            this.State = PortStates.Free;
            PortRejectReciving?.Invoke(sender, e);
        }

        #endregion

        private void SetStateFree(object obj, EventArgs args)
        {
            this.State = PortStates.Free;

            _terminal.Calling += OnPortCallSend;
            _terminal.Answering += OnPortAnswerSend;
            _terminal.Rejecting += OnPortRejectSend;

            this.PortCallReciving += _terminal.SetIncommingCallState;
            this.PortAnswerReciving += _terminal.SetOnlineState;
            this.PortRejectReciving += _terminal.SetConnectedState;
        }

        private void SetStateDisabled(object sender, EventArgs args)
        {
            this.State = PortStates.Disabled;

            _terminal.Calling -= OnPortCallSend;
            _terminal.Answering -= OnPortAnswerSend;
            _terminal.Rejecting -= OnPortRejectSend;

            this.PortCallReciving -= _terminal.SetIncommingCallState;
            this.PortAnswerReciving -= _terminal.SetOnlineState;
            this.PortRejectReciving -= _terminal.SetConnectedState;
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

        public void PortReciveReject(object sender, ICallingEventArgs e)
        {
            OnPortRejectRecive(sender, e);
        }

        public void ExemptPort()
        {
            SetStateDisabled(this, null);

            _terminal.Disconect();

            _terminal.Connecting -= this.SetStateFree;
            _terminal.Disconnecting -= this.SetStateDisabled;
            
        }
    }
}
