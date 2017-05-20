using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.Interfaces
{
    public interface IContract
    {
        string SubscriberFirstName { get; set; }

        string SubscriberLastName { get; set; }

        int TerminalNumber { get; set; }

        DateTime SignDate { get; set; }

        DateTime AbrogateDate { get; set; }

    }
}
