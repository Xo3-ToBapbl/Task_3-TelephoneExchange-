using ATE.Enums;
using ATE.EventArgsClasses;
using ATE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.HandlerClasses
{
    public class Station: IStation
    {
        public Station()
        {
            _mapping = new Dictionary<int, IPort>();
            _terminals = new List<ITerminal>();
        }

        private IList<ITerminal> _terminals;
        private IDictionary<int, IPort> _mapping;

        public IList<ITerminal> Terminals
        {
            get
            {
                return _terminals;
            }
        }

        public void AddMapItem(int number, IPort port)
        {
            port.PortStateChanging += this.DetectChanges;
            port.PortCallSending += this.HandlePortRequest;
            _mapping[number] = port;
        }

        public void AddTerminal(ITerminal terminal)
        {
            _terminals.Add(terminal);
        }


        public void DetectChanges(object obj, PortStates state)
        {
            Console.WriteLine("Station: port change state to '{0}'.", state);
        }

        private void HandlePortRequest(object sender, ICallingEventArgs e)
        {
            Console.WriteLine(
                "Station: port transfer call from terminal {0} to terminal {1}",
                e.SourceNumber, e.TargetNumber);

            if (_mapping.ContainsKey(e.TargetNumber))
            {
                IPort sourcePort = sender as IPort;
                IPort targetPort = _mapping[e.TargetNumber];

                if (targetPort.State == PortStates.Free)
                {
                    targetPort.PortReciveCall(targetPort, e);
                }
            }
        }
    }
}
