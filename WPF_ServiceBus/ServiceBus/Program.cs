using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WPF_ServiceBus.ServiceBus.model;
using WPF_ServiceBus.ServiceBus.session;

namespace WPF_ServiceBus.ServiceBus
{
    public class Program
    {
        private IServiceBusHandler _handler;
        private SessionData _SessionData = new SessionData();
        private SynchronizationContext _currentSynchronizationContext; // Needed to Synchronize between threads, Service buss handler is called from another thread

        public delegate void DataReceivedEventHandler(ConnectionModel source);
        public event DataReceivedEventHandler MessageReceived;

        public Program()
        {
            MessageReceived += DoNothing; // needed to prevent the application from crashing
            _currentSynchronizationContext = SynchronizationContext.Current;
        }

        private void DoNothing(ConnectionModel source){} // needed to prevent the application from crashing

        public void setResponse(ConnectionModel model)
        {
            MessageReceived(model);
        }

        public void SetData(Dictionary<string, string> data = null)
        {
            foreach (KeyValuePair<string, string> entry in data)
            {
                string key = entry.Key;
                string val = entry.Value;

                switch (key)
                {
                    case "ConnectionString":
                        Console.WriteLine($"ConnectionString: {val}");
                        _SessionData.connectionString = val; // Alternatively enter your connection string here.
                        break;
                    case "Topic":
                        Console.WriteLine($"Topic: {val}");
                        _SessionData.topic = val; // Alternatively enter your queue name here.
                        break;
                    case "Subscription":
                        Console.WriteLine($"Subscription: {val}");
                        _SessionData.subscription = val; // Alternatively enter your queue name here.
                        break;
                    case "Queue":
                        Console.WriteLine($"Queue: {val}");
                        _SessionData.queueName = val; // Alternatively enter your queue name here.
                        break;
                }
            }

            //_handler = new ServiceBusTopicHandler(_SessionData.connectionString, _SessionData.topic, _SessionData.subscription, ProcessMessagesAsync);
            _handler = new ServiceBusQueueHandler(_SessionData.connectionString, _SessionData.queueName, ProcessMessagesAsync);
        }

        public async void SendMessage(ConnectionModel connectionModel)
        {
            string line = JsonConvert.SerializeObject(connectionModel);
            await _handler.SendMessagesAsync(line);
        }

        public async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            // Process the message.
            string val = $"{Encoding.UTF8.GetString(message.Body)}";

            if (val.StartsWith("{") && val.EndsWith("}")){
                //convert the message to the model
                ConnectionModel connection = (ConnectionModel)JsonConvert.DeserializeObject(val, typeof(ConnectionModel));
                
                await _handler.completeAsync(message.SystemProperties.LockToken);

                _currentSynchronizationContext.Send(x => setResponse(connection), null);
            }
        }
        
    }
}
