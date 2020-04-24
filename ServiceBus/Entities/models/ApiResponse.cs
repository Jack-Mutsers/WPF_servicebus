using ServiceBus.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceBus.Entities.models
{
    public class ApiResponse
    {
        public int id { get; set; }
        public bool active { get; set; } = true;
        public string session_code { get; set; }
        public TopicData topicData { get; set; }
        public IEnumerable<Player> players { get; set; }
    }
}
