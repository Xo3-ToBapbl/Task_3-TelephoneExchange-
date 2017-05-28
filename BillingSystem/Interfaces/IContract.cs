using BillingSystem.Enums;
using System;

namespace BillingSystem.Interfaces
{
    public interface IContract
    {
        string SubscriberFirstName { get; set; }

        string SubscriberLastName { get; set; }

        int TerminalNumber { get; set; }

        TariffOption Tariff { get; set; }

        DateTime SignDate { get; set; }

        DateTime AbrogateDate { get; set; }

        bool ChangeTariff(TariffOption tariff);

    }
}
