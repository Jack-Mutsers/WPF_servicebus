﻿using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBus
{
    public class ServiceBusTopicHandler : IServiceBusHandler
    {
        ITopicClient topicClient;
        ISubscriptionClient subscriptionClient;

        public ServiceBusTopicHandler(string connectionString, string topic, string subscriptionName, Func<Message, CancellationToken, Task> onMessageRecivedCallBack)
        {
            topicClient = new TopicClient(connectionString, topic);
            subscriptionClient = new SubscriptionClient(connectionString, topic, subscriptionName);

            // Register subscription message handler and receive messages in a loop
            RegisterOnMessageHandlerAndReceiveMessages(onMessageRecivedCallBack);
        }

        public async Task SendMessagesAsync(string message)
        {
            try
            {
                // Create a new message to send to the topic.
                var encodedMessage = new Message(Encoding.UTF8.GetBytes(message));
                // Send the message to the topic.
                await topicClient.SendAsync(encodedMessage);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }

        void RegisterOnMessageHandlerAndReceiveMessages(Func<Message,CancellationToken,Task> onMessageRecivedCallBack)
        {
            // Configure the message handler options in terms of exception handling, number of concurrent messages to deliver, etc.
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Maximum number of concurrent calls to the callback ProcessMessagesAsync(), set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 1,

                // Indicates whether the message pump should automatically complete the messages after returning from user callback.
                // False below indicates the complete operation is handled by the user callback as in ProcessMessagesAsync().
                AutoComplete = false
            };

            // Register the function that processes messages.
            subscriptionClient.RegisterMessageHandler(onMessageRecivedCallBack, messageHandlerOptions);
        }

        public async Task completeAsync(string lockToken)
        {
            // Complete the message so that it is not received again.
            // This can be done only if the subscriptionClient is created in ReceiveMode.PeekLock mode (which is the default).
            await subscriptionClient.CompleteAsync(lockToken);
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