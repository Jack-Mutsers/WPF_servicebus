using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBus.Handlers
{
    public class ServiceBusTopicHandler : IServiceBusHandler
    {
        ITopicClient topicClient;
        ISubscriptionClient subscriptionClient;

        public ServiceBusTopicHandler(string connectionString, string topic, string subscriptionName, Func<IMessageSession ,Message, CancellationToken, Task> onMessageRecivedCallBack)
        {
            SetSubscriptionAsync(connectionString, topic, subscriptionName, onMessageRecivedCallBack);
        }

        public async void SetSubscriptionAsync(string connectionString, string topic, string subscriptionName, Func<IMessageSession, Message, CancellationToken, Task> onMessageRecivedCallBack)
        {
            if (topicClient != null)
            {
                await topicClient.CloseAsync();
            }
            
            if (subscriptionClient != null)
            {
                await subscriptionClient.CloseAsync();
            }

            // create connection link
            topicClient = new TopicClient(connectionString, topic);
            subscriptionClient = new SubscriptionClient(connectionString, topic, subscriptionName);

            // Register subscription message handler and receive messages in a loop
            RegisterOnMessageHandlerAndReceiveMessages(onMessageRecivedCallBack);
        }

        public async Task SendMessagesAsync(string message, string sessionCode)
        {
            try
            {
                // Create a new message to send to the topic.
                var encodedMessage = new Message(Encoding.UTF8.GetBytes(message));
                encodedMessage.SessionId = sessionCode;

                // Send the message to the topic.
                await topicClient.SendAsync(encodedMessage);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }

        void RegisterOnMessageHandlerAndReceiveMessages(Func<IMessageSession ,Message,CancellationToken,Task> onMessageRecivedCallBack)
        {
            var sessionHandlerOptions = new SessionHandlerOptions(ExceptionReceivedHandler)
            {
                //// Maximum number of concurrent calls to the callback ProcessMessagesAsync(), set to 1 for simplicity.
                //// Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentSessions = 100,

                //// Indicates whether MessagePump should automatically complete the messages after returning from User Callback.
                //// False below indicates the Complete will be handled by the User Callback as in `ProcessMessagesAsync` below.
                AutoComplete = true,
            };

            // Register the function that processes messages.
            subscriptionClient.RegisterSessionHandler(onMessageRecivedCallBack, sessionHandlerOptions);
        }

        public async void CloseConnection()
        {
            //await subscriptionClient.CloseAsync();
            //await topicClient.CloseAsync();
        }

        // Use this handler to examine the exceptions received on the message pump.
        Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }
    }
}
