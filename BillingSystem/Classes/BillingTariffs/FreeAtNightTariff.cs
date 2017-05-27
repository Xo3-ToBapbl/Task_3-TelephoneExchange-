using BillingSystem.Enums;
using BillingSystem.Extensions;
using BillingSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Classes.BillingTariffs
{
    public class FreeAtNightTariff: IBillingTariff
    {
        private TimeSpan _nightStart = new TimeSpan(0, 0, 0);
        private TimeSpan _nightEnd = new TimeSpan(12, 0, 0);

        public FreeAtNightTariff(int costPerMinute, TariffOption tariffType, int tariffCost)
        {
            CostPerMinute = costPerMinute;
            TariffType = tariffType;
            TariffCostPerMonth = tariffCost;
        }


        public int CostPerMinute { get; set; }

        public TariffOption TariffType { get; set; }

        public int TariffCostPerMonth { get; set; }


        public int CalculateCost(TimeSpan span, DateTime callTime)
        {
            if (callTime.TimeOfDay.IsBetween(_nightStart, _nightEnd))
            {
                return 0;
            }
            else
            {
                return Convert.ToInt16(span.TotalMinutes) * CostPerMinute;
            }
        }
    }
}
