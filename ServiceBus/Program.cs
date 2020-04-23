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

namespace ServiceBus
{
    public class Program
    {
        private IServiceBusTopicSessionHandler _ListnerTopicHandler;
        private IServiceBusTopicSessionHandler _WriterTopicHandler;
        private IServiceBusTopicHandler _TopicHandler;
        public TopicData TopicDataJoin { get; private set; }
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
            SetTopicData();
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

        public void CreateNewTopic()
        {
            TopicCreator creator = new TopicCreator();
            TopicData = creator.CreateNewTopic(TopicDataJoin.sessionCode);
            SetTopicData();
        }

        public void SetTopicJoinData(TopicData writerData, TopicData listnerData)
        {
            // set the session data
            TopicDataJoin = writerData;

            // convert subscription emum to string
            //string listnerSubscription = Enum.GetName(typeof(Subscriptions), listnerData.subscription);
            string writerSubscription = Enum.GetName(typeof(Subscriptions), writerData.subscription);

            // assign handler
            //_ListnerTopicHandler = new ServiceBusTopicSessionsHandler(listnerData.TopicConnectionString, listnerData.topic, listnerSubscription, ProcessTopicSessionAsync);
            _WriterTopicHandler = new ServiceBusTopicSessionsHandler(writerData.TopicConnectionString, writerData.topic, writerSubscription, ProcessTopicSessionAsync);
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
            await _WriterTopicHandler.SendMessagesAsync(line, TopicDataJoin.sessionCode);
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

        public async Task ProcessTopicSessionAsync(IMessageSession messageSession, Message message, CancellationToken token)
        {
            // check if the message is for me by compairing the session code
            if (TopicDataJoin.sessionCode != messageSession.SessionId)
            {
                return;
            }

            // Process the message.
            string val = $"{Encoding.UTF8.GetString(message.Body)}";

            // check if the message is json encoded
            if (val.StartsWith("{") && val.EndsWith("}"))
            {

                // decode the json
                Transfer transfer = JsonConvert.DeserializeObject<Transfer>(val);

                // check if the transfer type of the response type is and check if the application is still on the join subscription
                if (transfer.type == MessageType.Response)
                {
                    if (User.type != PlayerType.Host)
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
                    else
                    {
                        return;
                    }
                }

                // send message to the setResponse method
                _currentSynchronizationContext.Send(x => setResponse(val), null);
            }

            // complete the message so it is not recieved by anyone else
            await Task.CompletedTask;
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

    }
}
