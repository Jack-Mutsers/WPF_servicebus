using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBus.Handlers
{
    public class ServiceBusTopicSessionsHandler : IServiceBusTopicSessionHandler
    {
        ITopicClient topicClient;
        ISubscriptionClient subscriptionClient;

        public ServiceBusTopicSessionsHandler(string connectionString, string topic, string subscriptionName, Func<IMessageSession, Message, CancellationToken, Task> onMessageRecivedCallBack)
        {
            // create connection link
            topicClient = new TopicClient(connectionString, topic);
            subscriptionClient = new SubscriptionClient(connectionString, topic, subscriptionName);

            // Register subscription message handler and receive messages in a loop
            RegisterOnMessageHandlerAndReceiveMessages(onMessageRecivedCallBack);
        }
        
        public ServiceBusTopicSessionsHandler(string connectionString, string topic, string subscriptionName)
        {
            // create connection link
            topicClient = new TopicClient(connectionString, topic);
            subscriptionClient = new SubscriptionClient(connectionString, topic, subscriptionName);
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

        void RegisterOnMessageHandlerAndReceiveMessages(Func<IMessageSession, Message, CancellationToken, Task> onMessageRecivedCallBack)
        {
            var messageHandlerOptions = new SessionHandlerOptions(ExceptionReceivedHandler)
            {
                // Maximum number of concurrent calls to the callback ProcessMessagesAsync(), set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentSessions = 200,

                // Indicates whether the message pump should automatically complete the messages after returning from user callback.
                // False below indicates the complete operation is handled by the user callback as in ProcessMessagesAsync().
                AutoComplete = false
            };

            // Register the function that processes messages.
            subscriptionClient.RegisterSessionHandler(onMessageRecivedCallBack, messageHandlerOptions);
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
