using ServiceBus.Data;
using ServiceBus.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceBus.Entities.models
{
    public class Transfer
    {
        public QueueData QueueData { get; set; }
        public string message { get; set; }
        public MessageType type { get; set; }

    }
}
