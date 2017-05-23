using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.Interfaces
{
    public interface IStation
    {
        void AddMapItem(int number, IPort port, ITerminal terminal);
        void RemoveMapItem(int number);
        IList<ITerminal> Terminals { get; }
        IList<IPort> Ports { get; }
    }
}
