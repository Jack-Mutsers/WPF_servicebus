using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServiceBus.Entities.models;
using ServiceBus.session;
using System.Collections.Generic;
using System;
using Database.Entities.Enums;
using ServiceBus.Handlers;

namespace ServiceBus
{
    public class Program
    {
        private IServiceBusHandler _handler;
        public SessionData SessionData { get; private set; }
        public Player User { get; set; }

        private SynchronizationContext _currentSynchronizationContext; // Needed to Synchronize between threads, Service buss handler is called from another thread

        public delegate void DataReceivedEventHandler(string source);
        public event DataReceivedEventHandler MessageReceived;

        public Program()
        {
            MessageReceived += DoNothing;
            _currentSynchronizationContext = SynchronizationContext.Current;
        }

        public void UpdateSubscription(Subscriptions subscription)
        {
            // set assigned subscription
            SessionData.subscription = subscription;

            // convert subscription emum to string
            string subscriptionName = Enum.GetName(typeof(Subscriptions), SessionData.subscription);

            // assign handler
            _handler.SetSubscriptionAsync(SessionData.connectionString, SessionData.topic, subscriptionName, ProcessMessagesAsync);

            // create new handler with the new subscription
            //SetData(SessionData);
        }

        private void DoNothing(string message){ } // this is required for the event, so we can use the event in another class

        public void setResponse(string message)
        {
            // trigger the MessageRecieved event, so another class can handle the newly recieved data
            MessageReceived(message);
        }

        public void SetData(SessionData data)
        {
            // set the session data
            SessionData = data;

            if (_handler != null)
            {
                //_handler.CloseConnection();
            }

            // make sure handler is clear before setting a new one
            _handler = null;

            // convert subscription emum to string
            string subscriptionName = Enum.GetName(typeof(Subscriptions), SessionData.subscription);

            // assign handler
            _handler = new ServiceBusTopicHandler(SessionData.connectionString, SessionData.topic, subscriptionName, ProcessMessagesAsync);
            //_handler = new ServiceBusQueueHandler(_SessionData.connectionString, _SessionData.queueName, ProcessMessagesAsync);
        }

        public async void SendMessage(string message, MessageType type)
        {
            // create trasfer model to differentiate between message types
            Transfer transfer = new Transfer();
            transfer.message = message;
            transfer.type = type;

            // convert trasfer model to string for transfere
            string line = JsonConvert.SerializeObject(transfer);

            // sent the message string to the service bus
            await _handler.SendMessagesAsync(line, SessionData.sessionCode);
        }

        public async Task ProcessMessagesAsync(IMessageSession messageSession, Message message, CancellationToken token)
        {
            // check if the message is for me by compairing the session code
            if (SessionData.sessionCode != messageSession.SessionId)
            {
                return;
            }

            // Process the message.
            string val = $"{Encoding.UTF8.GetString(message.Body)}";

            // check if the message is json encoded
            if (val.StartsWith("{") && val.EndsWith("}") && _handler != null){

                // decode the json
                Transfer transfer = JsonConvert.DeserializeObject<Transfer>(val);

                // check if the transfer type of the response type is and check if the application is still on the join subscription
                if (transfer.type == MessageType.Response && SessionData.subscription == Subscriptions.Join)
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
