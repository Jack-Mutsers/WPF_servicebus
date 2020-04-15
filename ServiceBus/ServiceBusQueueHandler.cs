using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBus
{
    public class ServiceBusQueueHandler// : IServiceBusHandler
    {
        static IQueueClient _queueClient;

        public ServiceBusQueueHandler(string connectionString, string queueName, Func<IMessageSession, Message, CancellationToken, Task> onMessageRecivedCallBack)
        {
            // create connection link
            _queueClient = new QueueClient(connectionString, queueName);

            // Register QueueClient's MessageHandler and receive messages in a loop
            RegisterOnMessageHandlerAndReceiveMessages(onMessageRecivedCallBack);
        }

        public async Task SendMessagesAsync(string message, string sessionCode)
        {
            try
            {
                // Create a new message to send to the queue.
                var encodedMessage = new Message(Encoding.UTF8.GetBytes(message));

                // assign session code to message
                encodedMessage.SessionId = sessionCode;

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

        void RegisterOnMessageHandlerAndReceiveMessages(Func<IMessageSession, Message, CancellationToken, Task> onMessageRecivedCallBack)
        {
            // Configure the message handler options in terms of exception handling, number of concurrent messages to deliver, etc.
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
            _queueClient.RegisterSessionHandler(onMessageRecivedCallBack, sessionHandlerOptions);
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

        public void CloseConnection()
        {
            throw new NotImplementedException();
        }
    }
}