using ATE.Enums;
using ATE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.HandlerClasses
{
    public class Station: IStation
    {
        public Station()
        {
            _mapping = new Dictionary<int, IPort>();
            _terminals = new List<ITerminal>();
        }

        private ICollection<ITerminal> _terminals;
        private IDictionary<int, IPort> _mapping;

        public void AddMapItem(int number, IPort port)
        {
            _mapping[number] = port;
        }

        public void AddTerminal(ITerminal terminal)
        {
            _terminals.Add(terminal);
        }

        

        public void DetectChanges(object obj, PortState state)
        {
            Console.WriteLine("Station: port change state to '{0}'.", state);
        }
    }
}
