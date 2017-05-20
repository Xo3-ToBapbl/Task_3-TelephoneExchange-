using ATE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.Classes
{
    public class Station
    {
        public Station(Port port)
        {
            port.PortStateChanging += this.DetectChanges;
        }

        public void DetectChanges(object obj, PortState state)
        {
            Console.WriteLine("Station: port change state to '{0}'.", state);
        }
    }
}
