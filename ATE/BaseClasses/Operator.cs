using ATE.Enums;
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
        private IDictionary<int, IContract> _contracts;
        private IStation _station;

        public Operator(IStation station)
        {
            _contracts = new Dictionary<int, IContract>();
            _station = station;
        }

        public void SignContract(string firstName, string lastName, int terminalNumber, TariffOption tariff)
        {
            IContract newContract = new Contract(firstName, lastName, terminalNumber, tariff, DateTime.Now);

            _contracts[terminalNumber] = newContract;
            ITerminal newTerminal = ProduseTerminal(terminalNumber);
            IPort newPort = ProdusePort(newTerminal);

            //_station.AddTerminal(newTerminal);
            _station.AddMapItem(terminalNumber, newPort, newTerminal);
        }

        public void AbrogateContract(int terminalNumber)
        {
            if (_contracts.Keys.Contains(terminalNumber))
            {
                IContract contract = _contracts[terminalNumber];
                contract.AbrogateDate = DateTime.Now;

                _station.RemoveMapItem(terminalNumber);
            }
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
