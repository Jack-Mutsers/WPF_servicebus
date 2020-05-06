using ServiceBus.Entities.Enums;
using ServiceBus.Data;
using System;
using System.Collections.Generic;
using System.Text;
using ServiceBus.Resources;
using Microsoft.Azure.ServiceBus.Management;
using Microsoft.ServiceBus;

namespace ServiceBus.Manipulators
{
    public class TopicCreator
    {
        public TopicData CreateNewTopic()
        {
            // Configure Topic Settings.
            TopicDescription td = new TopicDescription("TestTopic");
            td.MaxSizeInMB = 5120;
            td.DefaultMessageTimeToLive = new TimeSpan(0, 1, 0);

            // Create the topic if it does not exist already.
            string connectionString = ServiceBusData.ConnectionString;

            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

            string topicName = "chat-" + StaticResources.sessionCode;


            if (namespaceManager.TopicExists(topicName))
            {
                return null;
            }

            namespaceManager.CreateTopic(topicName);

            foreach (Subscriptions subscription in (Subscriptions[])Enum.GetValues(typeof(Subscriptions)))
            {
                // convert subscription emum to string
                string subscriptionName = Enum.GetName(typeof(Subscriptions), subscription);
                CreateSubscription(topicName, subscriptionName);
            }

            TopicData data = new TopicData()
            {
                TopicConnectionString = connectionString,
                topic = topicName
            };

            return data;
        }

        private void CreateSubscription(string topicName, string subscriptionName)
        {
            string connectionString = ServiceBusData.ConnectionString;

            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

            if (!namespaceManager.SubscriptionExists(topicName, subscriptionName))
            {
                namespaceManager.CreateSubscription(topicName, subscriptionName);
            }
        }
    }
}
