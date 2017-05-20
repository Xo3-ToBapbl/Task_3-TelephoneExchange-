using ATE.Enums;
using ATE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.BaseClasses
{
    public class Contract : IContract
    {
        public Contract(
            string firstName, string lastName, int terminalNumber, TariffOption tariff, DateTime signDate)
        {
            SubscriberFirstName = firstName;
            SubscriberLastName = lastName;
            TerminalNumber = terminalNumber;
            Tariff = tariff;
            SignDate = signDate; 
        }

        public string SubscriberFirstName { get; set; }

        public string SubscriberLastName { get; set; }

        public int TerminalNumber { get; set; }

        public TariffOption Tariff { get; set; }

        public DateTime SignDate { get; set; }

        public DateTime AbrogateDate { get; set; }
    }
}
