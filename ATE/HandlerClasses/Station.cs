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
            _waitingConnection = new Dictionary<IPort, IPort>();
            _onConnection = new Dictionary<IPort, IPort>();
            _terminals = new List<ITerminal>();
        }

        private IList<ITerminal> _terminals;
        private IDictionary<int, IPort> _mapping;
        private IDictionary<IPort, IPort> _waitingConnection;
        private IDictionary<IPort, IPort> _onConnection;

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
            port.PortCallSending += this.HandlePortCallRequest;
            port.PortAnswerSending += this.HandlePortAnswerRequest;
            port.PortIgnoreSending += this.HandlePortIgnoreRequest;
            port.PortEndCallSending += this.HandlePortEndCallRequest;

            _mapping[number] = port;
        }

        public void AddTerminal(ITerminal terminal)
        {
            _terminals.Add(terminal);
        }


        public void DetectChanges(object sender, PortStates state)
        {
            Console.WriteLine("Station: port[{0}] change state to '{1}'.\n", 
                (sender as IPort).PortId, state);
        }

        private void HandlePortCallRequest(object sender, ICallingEventArgs e)
        {
            Console.WriteLine(
                "Station: port[{0}] transfer call from terminal {1} to terminal {2}.\n",
                (sender as IPort).PortId, e.SourceNumber, e.TargetNumber);

            if (_mapping.ContainsKey(e.TargetNumber))
            {
                IPort sourcePort = sender as IPort;
                IPort targetPort = _mapping[e.TargetNumber];
                _waitingConnection[targetPort] = sourcePort;

                if (targetPort.State == PortStates.Free)
                {
                    targetPort.PortReciveCall(targetPort, e);
                }
            }
        }

        private void HandlePortAnswerRequest(object sender, ICallingEventArgs e)
        {
            Console.WriteLine(
                "Station: port[{0}] transfer answer from terminal {2} to terminal {1}. Terminals online.\n",
                (sender as IPort).PortId, e.SourceNumber, e.TargetNumber);

            IPort sourcePort = _waitingConnection[sender as IPort];

            if (sourcePort.State == PortStates.Busy)
            {
                sourcePort.PortReciveAnswer(sourcePort, e);
            }
        }

        private void HandlePortIgnoreRequest(object sender, ICallingEventArgs e)
        {
            Console.WriteLine(
                "Station: port[{0}] transfer ignore proc from terminal {2} to terminal {1}.\n",
                (sender as IPort).PortId, e.SourceNumber, e.TargetNumber);

            IPort sourcePort = _waitingConnection[sender as IPort];

            if (sourcePort.State == PortStates.Busy)
            {
                sourcePort.PortReciveIgnore(sourcePort, e);
            }
        }

        private void HandlePortEndCallRequest(object sender, ICallingEventArgs e)
        {
            Console.WriteLine(
                "Station: port[{0}] transfer end call proc from terminal {2} to terminal {1}.\n",
                (sender as IPort).PortId, e.SourceNumber, e.TargetNumber);

            IPort sourcePort = _waitingConnection[sender as IPort];

            if (sourcePort.State == PortStates.Busy)
            {
                sourcePort.PortReciveEndCall(sourcePort, e);
            }
        }
    }
}
