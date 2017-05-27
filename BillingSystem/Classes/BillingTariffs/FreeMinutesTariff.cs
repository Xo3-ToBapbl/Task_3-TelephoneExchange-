using BillingSystem.Enums;
using BillingSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Classes.BillingTariffs
{
    public class FreeMinutesTariff : IBillingTariff
    {
        public FreeMinutesTariff(TimeSpan freeMinutes, int costPerMinute, TariffOption tariffType, int tariffCost)
        {
            FreeMinutes = freeMinutes;
            CurrentFreeMinutes = freeMinutes;
            CostPerMinute = costPerMinute;
            TariffType = tariffType;
            TariffCostPerMonth = tariffCost;
        }


        public TimeSpan FreeMinutes { get; set; }

        public TimeSpan CurrentFreeMinutes { get; set; }

        public int CostPerMinute { get; set; }

        public TariffOption TariffType { get; set; }

        public int TariffCostPerMonth { get; set; }


        public int CalculateCost(TimeSpan span, DateTime callTime)
        {
            if (CurrentFreeMinutes.TotalMinutes != 0)
            {
                CurrentFreeMinutes = FreeMinutes - span;
                if (CurrentFreeMinutes.TotalMinutes < 0)
                {
                    int cost = (-1) * Convert.ToInt16(CurrentFreeMinutes.TotalMinutes) * CostPerMinute;
                    CurrentFreeMinutes = TimeSpan.FromMinutes(0);
                    return cost;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return Convert.ToInt16(span.TotalMinutes) * CostPerMinute;
            }
        }
    }
}
