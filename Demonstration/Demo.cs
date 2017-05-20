using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATE.HandlerClasses;
using ATE.Interfaces;
using ATE.BaseClasses;
using ATE.Enums;

namespace Demonstration
{
    class Demo
    {
        static void Main(string[] args)
        {
            #region Subscribers:
            ISubscriber subscriber_1 = new Subscriber("Peter", "Parker");
            ISubscriber subscriber_2 = new Subscriber("Bill", "Murray");
            ISubscriber subscriber_3 = new Subscriber("Andrew", "Boget");
            #endregion

            IStation station = new Station();

            Operator operator_1 = new Operator(station);
            operator_1.SignContract(subscriber_1.FirstName, subscriber_1.LastName, 111111, TariffOption.Easy);
            operator_1.SignContract(subscriber_2.FirstName, subscriber_2.LastName, 222222, TariffOption.Standart);
            operator_1.SignContract(subscriber_3.FirstName, subscriber_3.LastName, 333333, TariffOption.Easy);

            ITerminal terminal_1 = station.Terminals[0];
            ITerminal terminal_2 = station.Terminals[1];
            ITerminal terminal_3 = station.Terminals[2];

            #region Tests:
            terminal_2.Connect();
            terminal_2.Call(333333);
            terminal_1.Connect();
            terminal_1.Call(555333);
            #endregion
            #region Close application
            Console.WriteLine("\nPress any key to close.");
            Console.ReadKey();
            #endregion
        }
    }
}
