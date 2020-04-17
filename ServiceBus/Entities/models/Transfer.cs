using Database.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceBus.Entities.models
{
    public class Transfer
    {
        public string message { get; set; }
        public MessageType type { get; set; }

    }
}
