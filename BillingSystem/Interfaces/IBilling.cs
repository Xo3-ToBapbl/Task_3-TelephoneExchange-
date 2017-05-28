using BillingSystem.Enums;
using System;
using System.Collections.Generic;

namespace BillingSystem.Interfaces
{
    public interface IBilling
    {
        IDictionary<int, ISubscriberInfo> Repository { get; }

        void AddStats(int number, IStatistic stat);
        void AddSubscriberInfo(int number, ISubscriberInfo info);
        void GetFullStatistic(int number);
        void GetFilteredStatistics(StatisticFilters filter, int number,
            int targetNumber=0, DateTime date = new DateTime(), ushort minCost = 0, ushort maxCost = 0);
    }
}
