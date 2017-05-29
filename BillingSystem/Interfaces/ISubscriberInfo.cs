using System;
using System.Collections.Generic;

namespace BillingSystem.Interfaces
{
    public interface ISubscriberInfo
    {
        IBillingTariff Tariff { get; set; }

        DateTime BillingPeriod { get; }

        IContract Contract { get; }

        ICollection<IStatistic> Statistics { get; }

        void AddStatistic(IStatistic statistic);
    }
}
