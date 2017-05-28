using BillingSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Classes
{
    public class SubscriberInfo: ISubscriberInfo
    {
        private int _subscriberNumber;
        private ICollection<IStatistic> _statistics;

        public SubscriberInfo(int number, IBillingTariff tariff, IContract contract)
        {
            _subscriberNumber = number;
            Tariff = tariff;
            Contract = contract;
            BillingPeriod = contract.SignDate;

            _statistics = new List<IStatistic>();
        }


        public IBillingTariff Tariff { get; set; }

        public DateTime BillingPeriod { get; }

        public IContract Contract { get; }

        public ICollection<IStatistic> Statistics
        {
            get
            {
                return _statistics;
            }
        }


        public void AddStatistic(IStatistic statistic)
        {
            if (statistic is IOutgoingCallStatistic)
            {
                IOutgoingCallStatistic outgoingCallStatistic =  statistic as IOutgoingCallStatistic;
                int cost = Tariff.CalculateCost(outgoingCallStatistic.Span, outgoingCallStatistic.StartTime);
                outgoingCallStatistic.Cost = cost;

                _statistics.Add(outgoingCallStatistic);
            }
            else
            {
                _statistics.Add(statistic);
            }
        }
    }
}
