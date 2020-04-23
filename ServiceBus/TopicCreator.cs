using ServiceBus.Entities.Enums;
using ServiceBus.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceBus
{
    public class TopicCreator
    {
        public TopicData CreateNewTopic(string session_code)
        {
            TopicData data = new TopicData();

            data.TopicConnectionString = "Endpoint=sb://fontysaquadis.servicebus.windows.net/;SharedAccessKeyName=fullcontroll;SharedAccessKey=5aT8daAKDusPqvesKYeWa2Y9GbTB8rmmh6lVVzS1MaU=;";
            data.topic = "nonsessionchat";
            data.subscription = Subscriptions.ChannelOne;

            return data;
        }
    }
}
