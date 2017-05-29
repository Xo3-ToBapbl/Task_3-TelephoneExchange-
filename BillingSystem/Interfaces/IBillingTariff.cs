using BillingSystem.Enums;
using System;

namespace BillingSystem.Interfaces
{
    public interface IBillingTariff
    {
        TariffOption TariffType { get; set; }
        int TariffCostPerMonth { get; set; }

        int CalculateCost(TimeSpan span, DateTime callTime);
    }
}
