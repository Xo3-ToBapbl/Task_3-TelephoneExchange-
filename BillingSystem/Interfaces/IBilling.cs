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
        void GetStatisticByDate(int number, DateTime date);
        void GetStatisticByCost(int number, ushort minCost, ushort maxCost);
        void GetStatisticByTargetAbonent(int number, int targetNumber);
    }
}
