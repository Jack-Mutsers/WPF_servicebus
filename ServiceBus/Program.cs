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
using ServiceBus.Resources;

namespace ServiceBus
{
    public class Program
    {
        private IServiceBusTopicHandler _TopicHandler;
        public TopicData TopicData { get; private set; }
        private SynchronizationContext _currentSynchronizationContext; // Needed to Synchronize between threads, Service buss handler is called from another thread

        public delegate void DataReceivedEventHandler(string source);
        public event DataReceivedEventHandler MessageReceived;

        public Program(TopicData data)
        {
            MessageReceived += DoNothing;
            _currentSynchronizationContext = SynchronizationContext.Current;

            // set topic data
            TopicData = data;

            // make sure handler is clear before setting a new one
            _TopicHandler = null;

            // convert subscription emum to string
            string subscriptionName = Enum.GetName(typeof(Subscriptions), TopicData.subscription);

            // assign handler
            _TopicHandler = new ServiceBusTopicHandler(TopicData.connectionString, TopicData.topic, subscriptionName, ProcessTopicMessagesAsync);

        }

        private void DoNothing(string message) { } // this is required for the event, so we can use the event in another class

        public void setResponse(string message)
        {
            // trigger the MessageRecieved event, so another class can handle the newly recieved data
            MessageReceived(message);
        }

        public async void SendTopicMessage(string message, MessageType type)
        {
            // create trasfer model to differentiate between message types
            Transfer transfer = new Transfer();
            transfer.message = message;
            transfer.type = type;

            // convert trasfer model to string for transfere
            string line = JsonConvert.SerializeObject(transfer);

            // sent the message string to the service bus
            await _TopicHandler.SendMessagesAsync(line);
        }

        public async Task ProcessTopicMessagesAsync(Message message, CancellationToken token)
        {
            // Process the message.
            string val = $"{Encoding.UTF8.GetString(message.Body)}";

            // check if the message is json encoded
            if (val.StartsWith("{") && val.EndsWith("}") && _TopicHandler != null)
            {

                // close recieved message
                await _TopicHandler.CompleteMessageAsync(message.SystemProperties.LockToken);

                // send message to the setResponse method
                _currentSynchronizationContext.Send(x => setResponse(val), null);
            }
        }

    }
}
