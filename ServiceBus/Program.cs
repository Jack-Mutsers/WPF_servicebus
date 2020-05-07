using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServiceBus.Entities.models;
using ServiceBus.Data;
using System.Collections.Generic;
using System;
using ServiceBus.Entities.Enums;
using ServiceBus.ServiceBusHandlers;
using ServiceBus.Manipulators;
using ServiceBus.Resources;
using ServiceBus.ConnectionHandlers;

namespace ServiceBus
{
    public class Program
    {
        public QueueListnerHandler QueueListner { get; private set; }
        public QueueWriterHandler QueueWriter { get; private set; }
        public TopicConnectionHandler topic { get; private set; }

        public Program(Player player)
        {
            StaticResources.user = player;
        }

        public void CreateQueueListner(PlayerType playerType)
        {
            QueueTypes queueTypes = new QueueTypes();

            string queueName = playerType == PlayerType.Host ?
                "Join-" + StaticResources.sessionCode :
                "response-" + StaticResources.sessionCode + StaticResources.user.userId.ToString();

            QueueData listnerData = QueueManipulator.CreateNewQueue(queueName);

            // pass over connection data
            QueueListner = new QueueListnerHandler(listnerData);
        }

        public void CreateQueueWriter(PlayerType playerType, QueueData queueData = null)
        {
            if (QueueWriter != null)
            {
                QueueWriter.DisconnectFromQueue();
            }

            QueueTypes queueTypes = new QueueTypes();

            QueueData writerData = queueData;

            if (playerType == PlayerType.Guest)
            {
                string queueName = "Join-" + StaticResources.sessionCode;
                writerData = QueueManipulator.CreateNewQueue(queueName);
            }

            // pass over connection data
            QueueWriter = new QueueWriterHandler(writerData);
        }

        public void CreateTopicConnection(TopicData data)
        {
            if (topic == null)
            {
                topic = new TopicConnectionHandler(data);
            }
        }

        public void CreateNewTopic()
        {
            TopicData data = TopicManipulator.CreateNewTopic();

            CreateTopicConnection(data);
        }

    }
}
