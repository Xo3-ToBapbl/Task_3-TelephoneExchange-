using ATE.Enums;
using ATE.Interfaces;
using BillingSystem.Classes;
using BillingSystem.Classes.Statistics;
using BillingSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ATE.HandlerClasses
{
    public class Station: IStation
    {
        private Random random = new Random();
        private DateTime _day;
        private TimeSpan _callSpan;
        private IBilling _billing;
        private IDictionary<int, ITerminal> _terminalMapping;
        private IDictionary<int, IPort> _portMapping;
        private IDictionary<IPort, IPort> _waitingConnection;   
        private IDictionary<IPort, IPort> _onConnection;

        public Station()
        {
            _portMapping = new Dictionary<int, IPort>();
            _terminalMapping = new Dictionary<int, ITerminal>();
            _waitingConnection = new Dictionary<IPort, IPort>();
            _onConnection = new Dictionary<IPort, IPort>();
        }


        public IBilling Billing
        {
            set
            {
                _billing = value;
            }
        }


        public ITerminal ReturnTerminal(int number)
        {
            if (_terminalMapping.ContainsKey(number))
            {
                return _terminalMapping[number];
            }
            else
            {
                Console.WriteLine("Terminal with number {0} does not exist.");
                return null;
            }
        }

        public void AddMapItem(int number, IPort port, ITerminal terminal)
        {
            port.PortStateChanging += this.DetectChanges;
            port.PortCallSending += this.HandlePortCallRequest;
            port.PortAnswerSending += this.HandlePortAnswerRequest;
            port.PortRejectSending += this.HandlePortRejectRequest;

            _portMapping[number] = port;
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

                _portMapping.Remove(number);
                _terminalMapping.Remove(number);
            }
        }

        
        public void DetectChanges(object sender, PortStates state)
        {
            Console.WriteLine("Station: port[{0}] change state to '{1}'.\n", 
                (sender as IPort).PortId, state);
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
                    CreateUnsuccessfulStats(e);
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

            Thread.Sleep(750);
            _callSpan = TimeSpan.FromMinutes(random.Next(1, 15));
            _day = DateTime.Now.AddDays(random.Next(0, 5));

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
            IStatistic sourceStat = new OutgoingCallStatistic(_day, _callSpan, e.TargetNumber);
            IStatistic targetStat = new IncomingCallStatistic(_day, _callSpan, e.SourceNumber);

            _billing.AddStats(e.SourceNumber, sourceStat);
            _billing.AddStats(e.TargetNumber, targetStat);
        }

        private void CreateUnsuccessfulStats(ICallingEventArgs e)
        {
            IStatistic sourceStat = new NotCalledStatistic(_day, e.TargetNumber);
            IStatistic targetStat = new MissedCallStatistic(_day, e.SourceNumber);

            _billing.AddStats(e.SourceNumber, sourceStat);
            _billing.AddStats(e.TargetNumber, targetStat);
        }
    }
}