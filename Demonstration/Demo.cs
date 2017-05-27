using System;
using ATE.HandlerClasses;
using ATE.Interfaces;
using Base;
using BillingSystem.Enums;
using BillingSystem.Interfaces;
using BillingSystem.Classes;

namespace Demonstration
{
    class Demo
    {
        static void Main(string[] args)
        {
            #region Subscribers:
            Subscriber subscriber_1 = new Subscriber("Peter", "Parker");
            Subscriber subscriber_2 = new Subscriber("Bill", "Murray");
            Subscriber subscriber_3 = new Subscriber("Andrew", "Boget");
            #endregion

            IBilling billing = new Billing();
            IStation station = new Station();
            Operator operator_ = new Operator(station, billing);

            #region Contracts:
            operator_.SignContract(subscriber_1.FirstName, subscriber_1.LastName, 111111, TariffOption.FreeMinutesEasy);
            operator_.SignContract(subscriber_2.FirstName, subscriber_2.LastName, 222222, TariffOption.FreeMinutesStandart);
            operator_.SignContract(subscriber_3.FirstName, subscriber_3.LastName, 333333, TariffOption.FreeMinutesEasy);
            #endregion
            #region Terminals, ports:
            IPort port_1 = station.Ports[0];
            IPort port_2 = station.Ports[1];
            IPort port_3 = station.Ports[2];

            ITerminal terminal_1 = station.Terminals[0];
            ITerminal terminal_2 = station.Terminals[1];
            ITerminal terminal_3 = station.Terminals[2];
            #endregion
            #region Tests:
            terminal_1.Connect();
            terminal_2.Connect();
            terminal_1.Call(222222);
            terminal_2.Answer();
            terminal_1.Reject();
            terminal_2.Call(111111);
            terminal_1.Reject();
            #endregion
            #region Close application
            Console.WriteLine("\nPress any key to close.");
            Console.ReadKey();
            #endregion
        }
    }
}
