using BillingSystem.Interfaces;
using System.Collections.Generic;

namespace ATE.Interfaces
{
    public interface IStation
    {
        void AddMapItem(int number, IPort port, ITerminal terminal);
        void RemoveMapItem(int number);
        IList<ITerminal> Terminals { get; }
        IList<IPort> Ports { get; }
        IBilling Billing { set; }
    }
}
