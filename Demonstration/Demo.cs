using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATE.HandlerClasses;
using ATE.Interfaces;
using ATE.BaseClasses;

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
            Contract contract_1 = new Contract(
                subscriber_1.FirstName, subscriber_1.LastName, "555-555", DateTime.Today);

            Operator operator_1 = new Operator();
            operator_1.SignContract(subscriber_1.FirstName, subscriber_1.LastName, 555111);
            operator_1.SignContract(subscriber_2.FirstName, subscriber_1.LastName, 555222);
            operator_1.SignContract(subscriber_3.FirstName, subscriber_1.LastName, 555333);

            #region Tests:
            //Terminal terminal_1 = new Terminal();
            //Port port_1 = new Port(terminal_1);

            //Station station = new Station(port_1);

            //terminal_1.Connect();
            //terminal_1.Disconect();
            #endregion
            #region Close application
            Console.WriteLine("Press any key to close.");
            Console.ReadKey();
            #endregion
        }
    }
}
