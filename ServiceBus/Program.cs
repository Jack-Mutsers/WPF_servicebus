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
using ServiceBus.Handlers;
using ServiceBus.Manipulators;
using ServiceBus.Resources;

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

        public void CreateQueueConnection(PlayerType playerType)
        {
            CreateQueueListner(playerType);
            CreateQueueWriter(playerType);
        }

        private void CreateQueueListner(PlayerType playerType)
        {
            QueueTypes queueTypes = new QueueTypes();
            QueueData listnerData = playerType == PlayerType.Host ?
                queueTypes.GetHostQueueListner() :
                queueTypes.GetGuestQueueListner();

            // pass over connection data
            QueueListner = new QueueListnerHandler(listnerData);
        }

        private void CreateQueueWriter(PlayerType playerType)
        {
            QueueTypes queueTypes = new QueueTypes();
            QueueData writerData = playerType == PlayerType.Host ?
                queueTypes.GetHostQueueWriter() :
                queueTypes.GetGuestQueueWriter();

            // pass over connection data
            QueueWriter = new QueueWriterHandler(writerData);
        }

        public void CreateTopic(TopicData data)
        {
            if (topic == null)
            {
                topic = new TopicConnectionHandler(data);
            }
        }

        public void CreateNewTopic()
        {
            TopicCreator topicCreator = new TopicCreator();
            TopicData data = topicCreator.CreateNewTopic();

            CreateTopic(data);
        }
    }
}
