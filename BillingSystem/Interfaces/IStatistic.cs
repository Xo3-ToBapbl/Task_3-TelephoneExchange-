using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Interfaces
{
    public interface IStatistic
    {
        string CallState { get; }
        int Cost { get; }
        int Number { get; }
        TimeSpan Span { get; }
        DateTime Start { get; }
        int TargetNumber { get; }
    }
}
