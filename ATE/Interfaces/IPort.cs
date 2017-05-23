﻿using ATE.Enums;
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
        PortStates State { get; set; }
        string PortId { get; }

        event EventHandler<PortStates> PortStateChanging;
        event EventHandler<ICallingEventArgs> PortCallSending;
        event EventHandler<ICallingEventArgs> PortAnswerSending;
        event EventHandler<ICallingEventArgs> PortRejectSending;

        void PortReciveCall(object sender, ICallingEventArgs e);
        void PortReciveAnswer(object sender, ICallingEventArgs e);
        void PortReciveReject(object sender, ICallingEventArgs e);

        void ExemptPort();
    }
}
