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
            _SessionData.subscription = subscription;
            SetData(_SessionData);
        }

        private void DoNothing(string message){}

        public void setResponse(string message)
        {
            MessageReceived(message);
        }

        public void SetData(SessionData data)
        {
            _SessionData = data;
            _handler = null;

            string subscription = Enum.GetName(typeof(Subscriptions), _SessionData.subscription);

            _handler = new ServiceBusTopicHandler(_SessionData.connectionString, _SessionData.topic, subscription, ProcessMessagesAsync);
            //_handler = new ServiceBusQueueHandler(_SessionData.connectionString, _SessionData.queueName, ProcessMessagesAsync);
        }

        public async void SendMessage(string message, MessageType type, string sessionCode)
        {
            TransferModel transfer = new TransferModel();
            transfer.sessionCode = sessionCode;
            transfer.message = message;
            transfer.type = type;

            string line = JsonConvert.SerializeObject(transfer);

            await _handler.SendMessagesAsync(line);
        }

        public async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            // Process the message.
            string val = $"{Encoding.UTF8.GetString(message.Body)}";

            if (val.StartsWith("{") && val.EndsWith("}")){
                //convert the message to the model
                //ActionModel connection = (ActionModel)JsonConvert.DeserializeObject(val, typeof(ActionModel));
                
                await _handler.completeAsync(message.SystemProperties.LockToken);

                _currentSynchronizationContext.Send(x => setResponse(val), null);
            }
        }
        
    }
}
