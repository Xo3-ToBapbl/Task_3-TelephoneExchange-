using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.Interfaces
{
    public interface IStation
    {
        void AddMapItem(int number, IPort port);
        void AddTerminal(ITerminal terminal);
        IList<ITerminal> Terminals { get; }
        IList<IPort> Ports { get; }
    }
}
