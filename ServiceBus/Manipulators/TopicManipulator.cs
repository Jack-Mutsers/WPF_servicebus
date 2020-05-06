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
    public static class TopicManipulator
    {
        public static TopicData CreateNewTopic()
        {
            // Configure Topic Settings.
            string topicName = "chat-" + StaticResources.sessionCode;

            TopicDescription td = new TopicDescription(topicName);
            td.MaxSizeInMB = 5120;
            td.DefaultMessageTimeToLive = new TimeSpan(0, 1, 0);

            // Create the topic if it does not exist already.
            string connectionString = ServiceBusData.ConnectionString;

            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

            if (!namespaceManager.TopicExists(topicName))
            {
                namespaceManager.CreateTopic(topicName);

                foreach (Subscriptions subscription in (Subscriptions[])Enum.GetValues(typeof(Subscriptions)))
                {
                    // convert subscription emum to string
                    string subscriptionName = Enum.GetName(typeof(Subscriptions), subscription);
                    CreateSubscription(topicName, subscriptionName);
                }
            }

            TopicData data = new TopicData()
            {
                TopicConnectionString = connectionString,
                topic = topicName
            };

            return data;
        }

        private static void CreateSubscription(string topicName, string subscriptionName)
        {
            string connectionString = ServiceBusData.ConnectionString;

            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

            if (!namespaceManager.SubscriptionExists(topicName, subscriptionName))
            {
                namespaceManager.CreateSubscription(topicName, subscriptionName);
            }
        }

        public static void DeleteTopic(string topicName)
        {
            string connectionString = ServiceBusData.ConnectionString;
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            namespaceManager.DeleteTopic(topicName);
        }
    }
}
