using Database.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceBus.Data
{
    public class TopicData
    {
        public string TopicConnectionString { get; set; }
        public string topic { get; set; } = "chat";
        public Subscriptions subscription { get; set; }
    }
}
