using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServiceBus.model;
using ServiceBus.session;
using System.Collections.Generic;
using System;

namespace ServiceBus
{
    public class Program
    {
        private IServiceBusHandler _handler;
        private SessionData _SessionData { get; set; }
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
            _SessionData.subscription = subscription;

            // create new handler with the new subscription
            SetData(_SessionData);
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
            _SessionData = data;

            // make sure handler is clear before setting a new one
            _handler = null;

            // convert subscription emum to string
            string subscription = Enum.GetName(typeof(Subscriptions), _SessionData.subscription);

            // assign handler
            _handler = new ServiceBusTopicHandler(_SessionData.connectionString, _SessionData.topic, subscription, ProcessMessagesAsync);
            //_handler = new ServiceBusQueueHandler(_SessionData.connectionString, _SessionData.queueName, ProcessMessagesAsync);
        }

        public async void SendMessage(string message, MessageType type, string sessionCode)
        {
            // create trasfer model to differentiate between message types
            TransferModel transfer = new TransferModel();
            transfer.sessionCode = sessionCode;
            transfer.message = message;
            transfer.type = type;

            // convert trasfer model to string for transfere
            string line = JsonConvert.SerializeObject(transfer);

            // sent the message string to the service bus
            await _handler.SendMessagesAsync(line);
        }

        public async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            // Process the message.
            string val = $"{Encoding.UTF8.GetString(message.Body)}";


            // check if the message is json encoded
            if (val.StartsWith("{") && val.EndsWith("}") && _handler != null){

                // notify the service bus that the message had been recieved
                await _handler.completeAsync(message.SystemProperties.LockToken);

                // send message to the setResponse method
                _currentSynchronizationContext.Send(x => setResponse(val), null);
            }
        }
        
    }
}
