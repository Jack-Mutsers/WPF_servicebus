using ServiceBus.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.Data
{
    public class QueueData
    {
        public string QueueConnectionString { get; set; }
        public string queueName { get; set; } = "join";
        public string sessionCode { get; set; }
    }
}
