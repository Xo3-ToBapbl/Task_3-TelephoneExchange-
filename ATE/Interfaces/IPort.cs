using ATE.Enums;
using ATE.EventArgsClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.Interfaces
{
    public interface IPort
    {
        event EventHandler<States> PortStateChanging;
        event EventHandler<ICallingEventArgs> PortCallTransfering;
    }
}
