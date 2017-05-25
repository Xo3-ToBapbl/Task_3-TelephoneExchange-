using BillingSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Classes
{
    public class Billing: IBilling
    {
        private IDictionary<int, IContract> _contracts;
        private IDictionary<int, ICollection<IStatistic>> _statistics;

        public Billing()
        {
            _contracts = new Dictionary<int, IContract>();
            _statistics = new Dictionary<int, ICollection<IStatistic>>();
        }

        public IDictionary<int, IContract> Contracts
        {
            get
            {
                return _contracts;
            }
        }


        public void Add(int number, IContract contract)
        {
            _contracts.Add(number, contract);
            _statistics.Add(number, new List<IStatistic>());
        }

        public void AddStats(int number, IStatistic stat)
        {
            if (_statistics.ContainsKey(number))
            {
                _statistics[number].Add(stat);
            }
            
        }
    }
}
