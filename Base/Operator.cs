using ATE.HandlerClasses;
using ATE.Interfaces;
using BillingSystem.Enums;
using BillingSystem.Interfaces;
using BillingSystem.Classes;
using System;

namespace Base
{
    public class Operator
    {
        private IStation _station;
        private IBilling _billing;

        public Operator(IStation station, IBilling billing)
        {
            _station = station;
            _billing = billing;
            _station.Billing = billing;
        }


        public void SignContract(string firstName, string lastName, int terminalNumber, TariffOption tariff)
        {
            IContract newContract = new Contract(firstName, lastName, terminalNumber, tariff, DateTime.Now);
            ITerminal newTerminal = ProduseTerminal(terminalNumber);
            IPort newPort = ProdusePort(newTerminal);

            _billing.Add(terminalNumber, newContract);
            _station.AddMapItem(terminalNumber, newPort, newTerminal);
        }

        public void AbrogateContract(int terminalNumber)
        {
            if (_billing.Contracts.Keys.Contains(terminalNumber))
            {
                IContract contract = _billing.Contracts[terminalNumber];
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
