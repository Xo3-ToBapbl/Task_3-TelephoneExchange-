using BillingSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BillingSystem.Classes
{
    public class Billing: IBilling
    {
        private IDictionary<int, ISubscriberInfo> _repository;

        public Billing()
        {
            _repository = new Dictionary<int, ISubscriberInfo>();
        }


        public IDictionary<int, ISubscriberInfo> Repository
        {
            get
            {
                return _repository;
            }
        }


        public void AddSubscriberInfo(int number, ISubscriberInfo info)
        {
            _repository.Add(number, info);
        }

        public void AddStats(int number, IStatistic stat)
        {
            if (_repository.ContainsKey(number))
            {
                _repository[number].AddStatistic(stat);
            }
        }
        

        public void GetFullStatistic(int number)
        {
            if(_repository.ContainsKey(number))
            {
                var orderedStats = _repository[number].Statistics.
                    Where(y=>y.StartTime >= DateTime.Now.AddMonths(-1)).
                    OrderBy(x => x.StartTime);

                if (orderedStats.Count() != 0)
                {
                    Console.WriteLine("\n\nFull statistic for abonent {0} {1}, terminal {2}:",
                        _repository[number].Contract.SubscriberFirstName, _repository[number].Contract.SubscriberLastName,
                        number);

                    PrintStatistic(orderedStats);
                    GetFullInfo(number);
                }
                else
                {
                    Console.WriteLine("\n\nThere are no statistics for abonent {0} {1}, terminal {2}.",
                        _repository[number].Contract.SubscriberFirstName, _repository[number].Contract.SubscriberLastName,
                        number);
                }
            }
            else
            {
                Console.WriteLine("\n\nThere is no abonent with number {0}.", number);
            }
        }

        public void GetStatisticByDate(int number, DateTime date)
        {
            if (_repository.ContainsKey(number))
            {
                var filteredStats = _repository[number].Statistics.Where(x => x.StartTime.Date == date.Date);
                if (filteredStats.Count() != 0)
                {
                    Console.WriteLine("\n\nFiltered by date {3} statistic for abonent {0} {1}, terminal {2}:",
                            _repository[number].Contract.SubscriberFirstName, _repository[number].Contract.SubscriberLastName,
                            number, date.Date);

                    PrintStatistic(filteredStats);
                }
                else
                {
                    Console.WriteLine("\n\nThere are no statistics for abonent {0} {1}, terminal {2}.",
                            _repository[number].Contract.SubscriberFirstName, _repository[number].Contract.SubscriberLastName,
                            number);
                }
            }
            else
            {
                Console.WriteLine("\n\nThere is no abonent with number {0}.", number);
            }
        }

        public void GetStatisticByCost(int number, ushort minCost, ushort maxCost)
        {
            if (_repository.ContainsKey(number))
            {
                if (minCost < maxCost)
                {
                    var filteredStats = _repository[number].Statistics.
                        Select(x => x as IOutgoingCallStatistic).
                        Where(y => (y != null) && (y.Cost >= minCost && y.Cost <= maxCost)).
                        OrderBy(z => z.Cost);

                    if (filteredStats.Count() != 0)
                    {
                        Console.WriteLine("\n\nFiltered by cost (from {3} to {4}) statistic for abonent {0} {1}, terminal {2}:",
                                _repository[number].Contract.SubscriberFirstName, _repository[number].Contract.SubscriberLastName,
                                number, minCost, maxCost);

                        PrintStatistic(filteredStats);
                    }
                    else
                    {
                        Console.WriteLine("\n\nThere are no statistics for abonent {0} {1}, terminal {2}.",
                                _repository[number].Contract.SubscriberFirstName, _repository[number].Contract.SubscriberLastName,
                                number);
                    }
                }
                else
                {
                    Console.WriteLine("\n\nIncorrect input for terminal {0}. Minimum cost must be less than maximum cost!",
                                number);
                }
            }
            else
            {
                Console.WriteLine("\n\nThere is no abonent with number {0}.", number);
            }
        }

        public void GetStatisticByTargetAbonent(int number, int targetNumber)
        {
            if (_repository.ContainsKey(number))
            {
                var filteredStats = _repository[number].Statistics.
                    Select(x => x as INotCalledStatistic).
                    Where(y => y != null && y.TargetNumber == targetNumber);

                if (filteredStats.Count() != 0)
                {
                    Console.WriteLine("\n\nFiltered by abonent {3} statistic for abonent {0} {1}, terminal {2}:",
                            _repository[number].Contract.SubscriberFirstName, _repository[number].Contract.SubscriberLastName,
                            number, targetNumber);

                    PrintStatistic(filteredStats);
                }
                else
                {
                    Console.WriteLine("\n\nThere are no statistics for abonent {0} {1}, terminal {2}.",
                            _repository[number].Contract.SubscriberFirstName, _repository[number].Contract.SubscriberLastName,
                            number);
                }
            }
            else
            {
                Console.WriteLine("\n\nThere is no abonent with number {0}.", number);
            }
        }

        private void GetFullInfo(int number)
        {
            var sum = _repository[number].Statistics.
                Where(y => y.StartTime >= DateTime.Now.AddMonths(-1) && y is IOutgoingCallStatistic).
                Cast<IOutgoingCallStatistic>().
                Sum(z=>z.Cost);
            Console.WriteLine("Tariff type: {0}, cost of calls for the last 30 days: {1}.",
                _repository[number].Tariff.TariffType, sum);
        }

        private void PrintStatistic(IEnumerable<IStatistic> statistics)
        {
            foreach (IStatistic stat in statistics)
            {
                if (stat is IOutgoingCallStatistic)
                {
                    IOutgoingCallStatistic outgoingCallStat = stat as IOutgoingCallStatistic;
                    Console.WriteLine("{0}: start at: {1}; spane: {2} minutes; cost {3}; target abonent {4}",
                        outgoingCallStat.CallState,
                        outgoingCallStat.StartTime, outgoingCallStat.Span,
                        outgoingCallStat.Cost, outgoingCallStat.TargetNumber);
                }
                else if (stat is IIncomingCallStatistic)
                {
                    IIncomingCallStatistic incomingCallStat = stat as IIncomingCallStatistic;
                    Console.WriteLine("{0}: start at: {1}; spane: {2} minutes; source abonent {3}",
                        incomingCallStat.CallState,
                        incomingCallStat.StartTime, incomingCallStat.Span, incomingCallStat.SourceNumber);
                }
                else if (stat is IMissedCallStatistic)
                {
                    IMissedCallStatistic missedCallStat = stat as IMissedCallStatistic;
                    Console.WriteLine("{0}: start at: {1}; source abonent {2}",
                        missedCallStat.CallState,
                        missedCallStat.StartTime, missedCallStat.SourceNumber);
                }
                else if (stat is INotCalledStatistic)
                {
                    INotCalledStatistic NotCalledStat = stat as INotCalledStatistic;
                    Console.WriteLine("{0}: start at: {1}; target abonent {2}",
                        NotCalledStat.CallState,
                        NotCalledStat.StartTime, NotCalledStat.TargetNumber);
                }
            }
        }
    }
}
