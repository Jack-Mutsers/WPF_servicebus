using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBus
{
    public class ServiceBusQueueHandler : IServiceBusHandler
    {
        static IQueueClient _queueClient;

        public ServiceBusQueueHandler(string connectionString, string queueName, Func<Message, CancellationToken, Task> onMessageRecivedCallBack)
        {
            _queueClient = new QueueClient(connectionString, queueName);

            // Register QueueClient's MessageHandler and receive messages in a loop
            RegisterOnMessageHandlerAndReceiveMessages(onMessageRecivedCallBack);
        }

        public async Task SendMessagesAsync(string message)
        {
            try
            {
                // Create a new message to send to the queue.
                var encodedMessage = new Message(Encoding.UTF8.GetBytes(message));

                // Write the body of the message to the console.
                Console.WriteLine($"Sending message: {message}");

                // Send the message to the queue.
                await _queueClient.SendAsync(encodedMessage);

            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }

        void RegisterOnMessageHandlerAndReceiveMessages(Func<Message, CancellationToken, Task> onMessageRecivedCallBack)
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
            _queueClient.RegisterMessageHandler(onMessageRecivedCallBack, messageHandlerOptions);
        }

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

        public async Task completeAsync(string lockToken)
        {
            // Complete the message so that it is not received again.
            // This can be done only if the queue Client is created in ReceiveMode.PeekLock mode (which is the default).
            await _queueClient.CompleteAsync(lockToken);
        }
    }
}