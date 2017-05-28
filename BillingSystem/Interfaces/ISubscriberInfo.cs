using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Interfaces
{
    public interface ISubscriberInfo
    {
        IBillingTariff Tariff { get; }

        DateTime BillingPeriod { get; }

        IContract Contract { get; }

        ICollection<IStatistic> Statistics { get; }

        void AddStatistic(IStatistic statistic);
    }
}
