using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServiceBus.model;
using ServiceBus.session;

namespace ServiceBus
{
    public class Program
    {
        private IServiceBusHandler _handler;
        private SessionData _SessionData { get; set; }
        private SynchronizationContext _currentSynchronizationContext; // Needed to Synchronize between threads, Service buss handler is called from another thread

        public delegate void DataReceivedEventHandler(ActionModel source);
        public event DataReceivedEventHandler MessageReceived;

        public Program()
        {
            MessageReceived += DoNothing; // needed to prevent the application from crashing
            _currentSynchronizationContext = SynchronizationContext.Current;
        }

        public void UpdateSubscription(Subscriptions subscription)
        {
            _SessionData.subscription = subscription;
        }

        private void DoNothing(ActionModel source){} // needed to prevent the application from crashing

        public void setResponse(ActionModel model)
        {
            MessageReceived(model);
        }

        public void SetData(SessionData data)
        {
            _SessionData = data;

            //_handler = new ServiceBusTopicHandler(_SessionData.connectionString, _SessionData.topic, _SessionData.subscription, ProcessMessagesAsync);
            _handler = new ServiceBusQueueHandler(_SessionData.connectionString, _SessionData.queueName, ProcessMessagesAsync);
        }

        public async void SendMessage(ActionModel connectionModel)
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
                ActionModel connection = (ActionModel)JsonConvert.DeserializeObject(val, typeof(ActionModel));
                
                await _handler.completeAsync(message.SystemProperties.LockToken);

                _currentSynchronizationContext.Send(x => setResponse(connection), null);
            }
        }
        
    }
}
