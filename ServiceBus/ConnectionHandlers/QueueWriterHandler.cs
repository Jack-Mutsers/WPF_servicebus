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

namespace ServiceBus.ConnectionHandlers
{
    public class QueueWriterHandler
    {
        private IServiceBusQueueHandler _WriterQueueHandler;
        public QueueData QueueData { get; private set; }

        private SynchronizationContext _currentSynchronizationContext; // Needed to Synchronize between threads, Service buss handler is called from another thread

        public QueueWriterHandler(QueueData writerData)
        {
            _currentSynchronizationContext = SynchronizationContext.Current;

            // set the session data
            QueueData = writerData;

            // assign handler
            _WriterQueueHandler = new ServiceBusQueueHandler(writerData.QueueConnectionString, writerData.queueName);

        }

        public async void SendQueueMessage(string message, MessageType type, QueueData queueData = null)
        {
            // create trasfer model to differentiate between message types
            Transfer transfer = new Transfer();
            transfer.message = message;
            transfer.type = type;
            transfer.QueueData = queueData;

            // convert trasfer model to string for transfere
            string line = JsonConvert.SerializeObject(transfer);

            // sent the message string to the service bus
            await _WriterQueueHandler.SendMessagesAsync(line, QueueData.sessionCode);
        }

        public void DisconnectFromQueue()
        {
            _WriterQueueHandler.DisconnectAsync();
        }
    }
}
