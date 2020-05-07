using Microsoft.Azure.ServiceBus.Management;
using Microsoft.ServiceBus;
using ServiceBus.Data;
using ServiceBus.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceBus.Manipulators
{
    public static class QueueManipulator
    {
        public static QueueData CreateNewQueue(string queueName)
        {
            // Configure Topic Settings.
            QueueDescription qd = new QueueDescription(queueName);
            qd.MaxSizeInMB = 5120;
            qd.DefaultMessageTimeToLive = new TimeSpan(0, 1, 0);

            // Create the topic if it does not exist already.
            string connectionString = ServiceBusData.ConnectionString;

            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

            if (!namespaceManager.QueueExists(queueName))
            {
                namespaceManager.CreateQueue(queueName);
            }

            QueueData data = new QueueData()
            {
                QueueConnectionString = connectionString,
                queueName = queueName,
            };

            return data;
        }

        public static void DeleteQueue(string queueName)
        {
            string connectionString = ServiceBusData.ConnectionString;
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            namespaceManager.DeleteQueue(queueName);
        }
    }
}
