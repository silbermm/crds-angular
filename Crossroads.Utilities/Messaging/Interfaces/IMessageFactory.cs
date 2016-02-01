﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Crossroads.Utilities.Messaging.Interfaces
{
    public interface IMessageFactory
    {
        Message CreateMessage(dynamic messageBody, IMessageFormatter formatter = null);
    }
}
