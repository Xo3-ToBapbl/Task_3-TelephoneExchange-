using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Interfaces
{
    public interface IBilling
    {
        IDictionary<int, IContract> Contracts { get; }

        void AddStats(int number, IStatistic stat);
        void Add(int number, IContract contract);
    }
}
