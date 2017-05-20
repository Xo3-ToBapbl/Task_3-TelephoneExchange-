using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATE.Classes;


namespace Demonstration
{
    class Demo
    {
        static void Main(string[] args)
        {
            Terminal terminal_1 = new Terminal();
            Port port_1 = new Port(terminal_1);

            Station station = new Station(port_1);

            terminal_1.Connect();
            terminal_1.Disconect();

            Console.ReadKey();
        }
    }
}
