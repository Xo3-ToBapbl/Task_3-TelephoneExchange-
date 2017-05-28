using BillingSystem.Enums;
using BillingSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Classes
{
    public class Contract : IContract
    {
        public Contract(string firstName, string lastName, int terminalNumber, 
            TariffOption tariff, DateTime signDate)
        {
            SubscriberFirstName = firstName;
            SubscriberLastName = lastName;
            TerminalNumber = terminalNumber;
            Tariff = tariff;
            SignDate = signDate; 
        }

        private DateTime TariffUpdateDate = new DateTime(1, 1, 1);

        public string SubscriberFirstName { get; set; }

        public string SubscriberLastName { get; set; }

        public int TerminalNumber { get; set; }

        public TariffOption Tariff { get; set; }

        public DateTime SignDate { get; set; }

        public DateTime AbrogateDate { get; set; }


        public bool ChangeTariff(TariffOption tariff)
        {
            if (TariffUpdateDate <= DateTime.Now.AddMonths(-1))
            {
                Tariff = tariff;
                Console.WriteLine("\nTariff successfully change.");
                return true;
            }
            else
            {
                Console.WriteLine("\nTariff already changed in this month.");
                return false;
            }
        }
    }
}
