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
            operator_.SignContract(subscriber_1.FirstName, subscriber_1.LastName, 111111, TariffOption.FreeAtNight);
            operator_.SignContract(subscriber_2.FirstName, subscriber_2.LastName, 222222, TariffOption.FreeMinutesStandart);
            operator_.SignContract(subscriber_3.FirstName, subscriber_3.LastName, 333333, TariffOption.FreeMinutesEasy);
            #endregion

            #region Terminals:
            ITerminal terminal_1 = station.ReturnTerminal(111111);
            ITerminal terminal_2 = station.ReturnTerminal(222222);
            ITerminal terminal_3 = station.ReturnTerminal(333333);
            #endregion

            #region Tests:
            terminal_1.Connect();
            terminal_2.Connect();
            terminal_3.Connect();

            terminal_2.Call(333333);
            terminal_3.Answer();
            terminal_3.Reject();

            operator_.ChangeTariff(222222, TariffOption.FreeAtNight);

            terminal_2.Call(111111);
            terminal_1.Answer();
            terminal_2.Reject();

            terminal_3.Call(111111);
            terminal_1.Reject();

            terminal_2.Call(111111);
            terminal_1.Reject();

            terminal_2.Call(333333);
            terminal_3.Answer();
            terminal_3.Reject();

            terminal_1.Call(333333);
            terminal_3.Answer();
            terminal_3.Reject();

            terminal_1.Call(222222);
            terminal_2.Answer();
            terminal_1.Reject();

            terminal_3.Call(222222);
            terminal_2.Answer();
            terminal_2.Reject();

            terminal_3.Call(111111);
            terminal_1.Reject();

            billing.GetFullStatistic(111111);
            billing.GetFullStatistic(222222);
            billing.GetFullStatistic(333333);
            #endregion
            #region Close application
            Console.WriteLine("\nPress any key to close.");
            Console.ReadKey();
            #endregion
        }
    }
}
