using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServiceBus.Entities.models;
using ServiceBus.Data;
using System.Collections.Generic;
using System;
using Database.Entities.Enums;
using ServiceBus.Handlers;

namespace ServiceBus
{
    public class Program
    {
        private IServiceBusQueueHandler _QueueHandler;
        private IServiceBusTopicHandler _TopicHandler;
        public QueueData QueueData { get; private set; }
        public TopicData TopicData { get; private set; }
        public Player User { get; set; }

        private SynchronizationContext _currentSynchronizationContext; // Needed to Synchronize between threads, Service buss handler is called from another thread

        public delegate void DataReceivedEventHandler(string source);
        public event DataReceivedEventHandler MessageReceived;

        public Program()
        {
            MessageReceived += DoNothing;
            _currentSynchronizationContext = SynchronizationContext.Current;
        }

        private void DoNothing(string message){ } // this is required for the event, so we can use the event in another class

        public void setResponse(string message)
        {
            // trigger the MessageRecieved event, so another class can handle the newly recieved data
            MessageReceived(message);
        }

        public void StoreTopicData(TopicData data)
        {
            // set the session data
            TopicData = data;
        }

        public void SetTopicData()
        {
            // make sure handler is clear before setting a new one
            _TopicHandler = null;

            // convert subscription emum to string
            string subscriptionName = Enum.GetName(typeof(Subscriptions), TopicData.subscription);

            // assign handler
            _TopicHandler = new ServiceBusTopicHandler(TopicData.TopicConnectionString, TopicData.topic, subscriptionName, ProcessTopicMessagesAsync);
        }

        public void SetQueueData(QueueData data)
        {
            // set the session data
            QueueData = data;

            // make sure handler is clear before setting a new one
            _QueueHandler = null;

            // assign handler
            _QueueHandler = new ServiceBusQueueHandler(data.QueueConnectionString, data.queueName, ProcessQueueSessionAsync);
        }

        public async void SendQueueMessage(string message, MessageType type)
        {
            // create trasfer model to differentiate between message types
            Transfer transfer = new Transfer();
            transfer.message = message;
            transfer.type = type;

            // convert trasfer model to string for transfere
            string line = JsonConvert.SerializeObject(transfer);

            // sent the message string to the service bus
            await _QueueHandler.SendMessagesAsync(line, QueueData.sessionCode);
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
            if (val.StartsWith("{") && val.EndsWith("}") && _TopicHandler != null){

                // close recieved message
                await _TopicHandler.CompleteMessageAsync(message.SystemProperties.LockToken);

                // send message to the setResponse method
                _currentSynchronizationContext.Send(x => setResponse(val), null);
            }
        }

        public async Task ProcessQueueSessionAsync(IMessageSession messageSession, Message message, CancellationToken token)
        {
            // check if the message is for me by compairing the session code
            if (QueueData.sessionCode != messageSession.SessionId)
            {
                return;
            }

            // Process the message.
            string val = $"{Encoding.UTF8.GetString(message.Body)}";

            // check if the message is json encoded
            if (val.StartsWith("{") && val.EndsWith("}") && _QueueHandler != null)
            {

                // decode the json
                Transfer transfer = JsonConvert.DeserializeObject<Transfer>(val);

                // check if the transfer type of the response type is and check if the application is still on the join subscription
                if (transfer.type == MessageType.Response)
                {
                    // I am still waiting for my session response
                    // decode the response
                    var responseModel = JsonConvert.DeserializeObject<SessionResponse>(transfer.message);

                    // check if the response is meant for me
                    if (responseModel.Player.userId != User.userId)
                    {
                        // the response was not for me
                        return;
                    }
                }

                // send message to the setResponse method
                _currentSynchronizationContext.Send(x => setResponse(val), null);
            }

            // complete the message so it is not recieved by anyone else
            await Task.CompletedTask;
        }
    }
}
