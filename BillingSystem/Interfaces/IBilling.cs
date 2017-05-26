using System.Collections.Generic;

namespace BillingSystem.Interfaces
{
    public interface IBilling
    {
        IDictionary<int, IContract> Contracts { get; }

        void AddStats(int number, IStatistic stat);
        void Add(int number, IContract contract);
    }
}
