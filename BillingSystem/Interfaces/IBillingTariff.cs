using BillingSystem.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Interfaces
{
    public interface IBillingTariff
    {
        TariffOption TariffType { get; set; }
        int TariffCostPerMonth { get; set; }

        int CalculateCost(TimeSpan span, DateTime callTime);
    }
}
