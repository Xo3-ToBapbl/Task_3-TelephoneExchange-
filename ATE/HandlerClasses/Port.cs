using ATE.Enums;
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
        public Port(ITerminal terminal)
        {
            terminal.Connecting += this.SetStateFree;
            terminal.Disconnecting += this.SetStateDisabled;
        }

        private PortState _state = PortState.Disabled;

        public PortState State
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

        public event EventHandler<PortState> PortStateChanging;

        #region Start event methods
        protected virtual void OnPortStateChanged(object sender, PortState state)
        {
            PortStateChanging?.Invoke(sender, state);
        }
        #endregion

        private void SetStateFree(object obj, EventArgs args)
        {
            this.State = PortState.Free;
        }

        private void SetStateDisabled(object obj, EventArgs args)
        {
            this.State = PortState.Disabled;
        }
    }
}
