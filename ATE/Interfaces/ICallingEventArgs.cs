using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.Interfaces
{
    public interface ICallingEventArgs
    {
        int SourceNumber { get; }
        int TargetNumber { get; }
    }
}
