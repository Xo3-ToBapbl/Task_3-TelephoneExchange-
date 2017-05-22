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
            _ports = new List<IPort>();
        }

        private IList<ITerminal> _terminals;
        private IList<IPort> _ports;
        private IDictionary<int, IPort> _mapping;
        private IDictionary<IPort, IPort> _waitingConnection;   // key - who call, value - whom call
        private IDictionary<IPort, IPort> _onConnection;        // key - who call, value - whom call

        public IList<ITerminal> Terminals
        {
            get
            {
                return _terminals;
            }
        }

        public IList<IPort> Ports
        {
            get
            {
                return _ports;
            }
        }

        public void AddMapItem(int number, IPort port)
        {
            port.PortStateChanging += this.DetectChanges;
            port.PortCallSending += this.HandlePortCallRequest;
            port.PortAnswerSending += this.HandlePortAnswerRequest;
            port.PortRejectSending += this.HandlePortRejectRequest;

            _ports.Add(port);
            _mapping[number] = port;
        }

        public void AddTerminal(ITerminal terminal)
        {
            _terminals.Add(terminal);
        }


        public void DetectChanges(object sender, PortStates state)
        {
            //Console.WriteLine("Station: port[{0}] change state to '{1}'.\n", 
                //(sender as IPort).PortId, state);
        }

        private void HandlePortCallRequest(object sender, ICallingEventArgs e)
        {
            if (_mapping.ContainsKey(e.TargetNumber))
            {
                Console.WriteLine(
                "Station: port[{0}] transfer call from terminal {1} to terminal {2}.\n",
                (sender as IPort).PortId, e.SourceNumber, e.TargetNumber);

                IPort sourcePort = sender as IPort;
                IPort targetPort = _mapping[e.TargetNumber];

                _waitingConnection[sourcePort] = targetPort;

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

            IPort targetPort = sender as IPort;
            IPort sourcePort = _mapping[e.SourceNumber];

            if (sourcePort.State == PortStates.Busy)
            {
                _waitingConnection.Remove(targetPort); _waitingConnection.Remove(sourcePort);
                _onConnection[sourcePort] = targetPort;
                sourcePort.PortReciveAnswer(targetPort, e);
            }
        }

        private void HandlePortRejectRequest(object sender, ICallingEventArgs e)
        {
            IPort port = sender as IPort;

            if (_waitingConnection.Values.Contains(port))
            {
                HandleIgnoreRequest(port, e);
            }
            else if (_onConnection.Keys.Contains(port))
            {
                HandleRejectRequestFromSourceTerminal(port, e);
            }
            else if (_onConnection.Values.Contains(port))
            {
                HandleRejectRequestFromTargetTerminal(port, e);
            }
        }

        
        private void HandleIgnoreRequest(IPort port, ICallingEventArgs e)
        {// Ignore request only from 'target' terminal
            IPort targetPort = port;
            IPort sourcePort = _waitingConnection.FirstOrDefault(x => x.Value == port).Key;
            _waitingConnection.Remove(sourcePort);

            Console.WriteLine(
                "Station: port[{0}] transfer reject from terminal {2} to terminal {1}.\n",
                targetPort.PortId, e.SourceNumber, e.TargetNumber);

            sourcePort.PortReciveReject(sourcePort, e);
        }

        private void HandleRejectRequestFromSourceTerminal(IPort port, ICallingEventArgs e)
        {
            IPort sourcePort = port;
            IPort targetPort = _onConnection[sourcePort];
            
            Console.WriteLine(
                "Station: port[{0}] transfer reject from terminal {1} to terminal {2}.\n",
                sourcePort.PortId, e.SourceNumber, e.TargetNumber);

            targetPort.PortReciveReject(targetPort, e);
        }

        private void HandleRejectRequestFromTargetTerminal(IPort port, ICallingEventArgs e)
        {
            IPort sourcePort = _onConnection.FirstOrDefault(x => x.Value == port).Key;
            IPort targetPort = port;

            Console.WriteLine(
                "Station: port[{0}] transfer reject from terminal {2} to terminal {1}.\n",
                targetPort.PortId, e.SourceNumber, e.TargetNumber);

            sourcePort.PortReciveReject(sourcePort, e);
        }
    }
}