using BillingSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Classes
{
    class SubscriberInfo
    {
        private int _subscriberNumber;

        public SubscriberInfo(int number)
        {
            _subscriberNumber = number;
        }


        IBillingTariff Tariff { get; }

        public DateTime BillingPeriod { get; }

        public IContract Contract { get; }

        public ICollection<IStatistic> Statistics { get; }

    }
}
