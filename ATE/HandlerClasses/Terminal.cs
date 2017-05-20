using ATE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.HandlerClasses
{
    public class Terminal: ITerminal
    {
        public Terminal(int number)
        {
            _number = number;
        }

        private int _number;

        public event EventHandler Connecting;
        public event EventHandler Disconnecting;

        #region Start event methods:
        protected virtual void OnConnecting(object sender, EventArgs args)
        {
            Connecting?.Invoke(sender, args);
        }

        protected virtual void OnDisconnecting(object sender, EventArgs args)
        {
            Disconnecting?.Invoke(sender, args);
        }
        #endregion

        public void Connect()
        {
            OnConnecting(this, null);
        }   

        public void Disconect()
        {
            OnDisconnecting(this, null);
        }
    }
}
