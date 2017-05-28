using ATE.HandlerClasses;
using ATE.Interfaces;
using BillingSystem.Enums;
using BillingSystem.Interfaces;
using BillingSystem.Classes;
using System;
using BillingSystem.Classes.BillingTariffs;

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


        public void SignContract(string firstName, string lastName, int terminalNumber, TariffOption tariffType)
        {
            IContract newContract = new Contract(firstName, lastName, terminalNumber, tariffType, DateTime.Now);
            ITerminal newTerminal = ProduseTerminal(terminalNumber);
            IPort newPort = ProdusePort(newTerminal);
            ISubscriberInfo subscriberInfo = CreateSubscriberInfo(tariffType, terminalNumber, newContract);

            _billing.AddSubscriberInfo(terminalNumber, subscriberInfo);
            _station.AddMapItem(terminalNumber, newPort, newTerminal);
        }

        public void AbrogateContract(int terminalNumber)
        {
            if (_billing.Repository.Keys.Contains(terminalNumber))
            {
                IContract contract = _billing.Repository[terminalNumber].Contract;
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

        private ISubscriberInfo CreateSubscriberInfo(TariffOption tariffType, int number, IContract contract)
        {
            IBillingTariff tariff;

            switch(tariffType)
            {
                case TariffOption.FreeAtNight:
                    {
                        tariff = new FreeAtNightTariff(5, TariffOption.FreeAtNight, 100);
                        break;
                    }
                case TariffOption.FreeMinutesEasy:
                    {
                        tariff = new FreeMinutesTariff(new TimeSpan(0,50,0), 3, TariffOption.FreeMinutesEasy, 120);
                        break;
                    }
                case TariffOption.FreeMinutesStandart:
                    {
                        tariff = new FreeMinutesTariff(new TimeSpan(0, 100, 0), 2, TariffOption.FreeMinutesStandart, 150);
                        break;
                    }
                default:
                    {
                        tariff = new FreeMinutesTariff(new TimeSpan(0, 100, 0), 2, TariffOption.FreeMinutesStandart, 150);
                        break;
                    }
            }

            return new SubscriberInfo(number, tariff, contract);
        }
    }
}
