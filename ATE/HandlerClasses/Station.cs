using ATE.Enums;
using ATE.Interfaces;
using BillingSystem.Classes;
using BillingSystem.Classes.Statistics;
using BillingSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATE.HandlerClasses
{
    public class Station: IStation
    {
        private static Random random = new Random();
        private IBilling _billing;
        private IList<ITerminal> _terminals;
        private IList<IPort> _ports;
        private IDictionary<int, ITerminal> _terminalMapping;
        private IDictionary<int, IPort> _portMapping;
        private IDictionary<IPort, IPort> _waitingConnection;   // key - who call, value - whom call
        private IDictionary<IPort, IPort> _onConnection;        // key - who call, value - whom call

        public Station()
        {
            _portMapping = new Dictionary<int, IPort>();
            _terminalMapping = new Dictionary<int, ITerminal>();
            _waitingConnection = new Dictionary<IPort, IPort>();
            _onConnection = new Dictionary<IPort, IPort>();
            _terminals = new List<ITerminal>();
            _ports = new List<IPort>();
        }

        
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

        public IBilling Billing
        {
            set
            {
                _billing = value;
            }
        }


        public void AddMapItem(int number, IPort port, ITerminal terminal)
        {
            port.PortStateChanging += this.DetectChanges;
            port.PortCallSending += this.HandlePortCallRequest;
            port.PortAnswerSending += this.HandlePortAnswerRequest;
            port.PortRejectSending += this.HandlePortRejectRequest;

            _ports.Add(port);
            _portMapping[number] = port;

            _terminals.Add(terminal);
            _terminalMapping[number] = terminal;
        }

        public void RemoveMapItem(int number)
        {
            
            IPort port = _portMapping[number];
            if (port.State != PortStates.Busy)
            {
                ITerminal terminal = _terminalMapping[number];

                port.ExemptPort();

                port.PortStateChanging -= this.DetectChanges;
                port.PortCallSending -= this.HandlePortCallRequest;
                port.PortAnswerSending -= this.HandlePortAnswerRequest;
                port.PortRejectSending -= this.HandlePortRejectRequest;

                _terminals.Remove(terminal);
                _ports.Remove(port);
                _portMapping.Remove(number);
                _terminalMapping.Remove(number);
            }
        }

        
        public void DetectChanges(object sender, PortStates state)
        {
            //Console.WriteLine("Station: port[{0}] change state to '{1}'.\n", 
                //(sender as IPort).PortId, state);
        }


        private void HandlePortCallRequest(object sender, ICallingEventArgs e)
        {
            IPort sourcePort = sender as IPort;

            if (_portMapping.ContainsKey(e.TargetNumber))
            {
                IPort targetPort = _portMapping[e.TargetNumber];

                if (targetPort.State == PortStates.Free)
                {
                    Console.WriteLine("Station: port[{0}] transfer call from terminal {1} to terminal {2}.\n",
                        sourcePort.PortId, e.SourceNumber, e.TargetNumber);

                    _waitingConnection[sourcePort] = targetPort;
                    targetPort.PortReciveCall(targetPort, e);
                }
                else
                {
                    Console.WriteLine(
                        "Station to port[{0}]: call from terminal {1} to terminal {2} turn dowm. Terminal busy or disconected.\n",
                        sourcePort.PortId, e.SourceNumber, e.TargetNumber);

                    sourcePort.PortReciveReject(sourcePort, e);
                }
            }
            else
            {
                Console.WriteLine("Station to port[{0}]: invalid target number, or number is not exist.\n", 
                    sourcePort.PortId);

                sourcePort.PortReciveReject(sourcePort, e);
            }
        }

        private void HandlePortAnswerRequest(object sender, ICallingEventArgs e)
        {
            Console.WriteLine(
                "Station: port[{0}] transfer answer from terminal {2} to terminal {1}. Terminals online.\n",
                (sender as IPort).PortId, e.SourceNumber, e.TargetNumber);

            IPort targetPort = sender as IPort;
            IPort sourcePort = _portMapping[e.SourceNumber];

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
                CreateUnsuccessfulStats(e);
            }
            else if (_onConnection.Keys.Contains(port))
            {
                HandleRejectRequestFromSourceTerminal(port, e);
                CreateSuccessfulStats(e);
            }
            else if (_onConnection.Values.Contains(port))
            {
                HandleRejectRequestFromTargetTerminal(port, e);
                CreateSuccessfulStats(e);
            }
        }

        
        private void HandleIgnoreRequest(IPort port, ICallingEventArgs e)
        {
            // Ignore request only from 'target' terminal

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

            _onConnection.Remove(sourcePort);
            targetPort.PortReciveReject(targetPort, e);
        }

        private void HandleRejectRequestFromTargetTerminal(IPort port, ICallingEventArgs e)
        {
            IPort sourcePort = _onConnection.FirstOrDefault(x => x.Value == port).Key;
            IPort targetPort = port;

            Console.WriteLine(
                "Station: port[{0}] transfer reject from terminal {2} to terminal {1}.\n",
                targetPort.PortId, e.SourceNumber, e.TargetNumber);

            _onConnection.Remove(sourcePort);
            sourcePort.PortReciveReject(sourcePort, e);
        }


        private void CreateSuccessfulStats(ICallingEventArgs e)
        {
            TimeSpan callSpan = TimeSpan.FromMinutes(random.Next(1, 15));
            DateTime day = DateTime.Now.AddDays(random.Next(0, 10));

            IStatistic sourceStat = new OutgoingCallStatistic(day, callSpan, e.TargetNumber);
            IStatistic targetStat = new IncomingCallStatistic(day, callSpan, e.SourceNumber);

            _billing.AddStats(e.SourceNumber, sourceStat);
            _billing.AddStats(e.TargetNumber, targetStat);
        }

        private void CreateUnsuccessfulStats(ICallingEventArgs e)
        {
            TimeSpan callSpan = TimeSpan.FromMinutes(random.Next(1, 15));
            DateTime day = DateTime.Now.AddDays(random.Next(0, 10));

            IStatistic sourceStat = new NotCalledStatistic(day, e.TargetNumber);
            IStatistic targetStat = new MissedCallStatistic(day, e.SourceNumber);

            _billing.AddStats(e.SourceNumber, sourceStat);
            _billing.AddStats(e.TargetNumber, targetStat);
        }
    }
}