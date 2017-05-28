using BillingSystem.Interfaces;
using System.Collections.Generic;

namespace ATE.Interfaces
{
    public interface IStation
    {
        IBilling Billing { set; }

        void AddMapItem(int number, IPort port, ITerminal terminal);
        void RemoveMapItem(int number);
        ITerminal ReturnTerminal(int number);
        
    }
}
