using ATE.HandlerClasses;
using ATE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.BaseClasses
{
    public class Operator
    {
        private IDictionary<Guid, IContract> _contracts;
        private IStation _station;

        public Operator(IStation station)
        {
            _contracts = new Dictionary<Guid, IContract>();
            _station = station;
        }

        public void SignContract(string firstName, string lastName, int terminalNumber)
        {
            IContract newContract = new Contract(firstName, lastName, terminalNumber, DateTime.Now);
            Guid id = Guid.NewGuid();

            _contracts[id] = newContract;
            ITerminal newTerminal = ProduseTerminal(terminalNumber);
            IPort newPort = ProdusePort(newTerminal);

            _station.AddTerminal(newTerminal);
            _station.AddMapItem(terminalNumber, newPort);
        }

        private ITerminal ProduseTerminal(int number)
        {
            return new Terminal(number);
        }

        private IPort ProdusePort(ITerminal terminal)
        {
            return new Port(terminal);
        }
    }
}
