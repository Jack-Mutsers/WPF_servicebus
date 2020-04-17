using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBus
{
    public interface IServiceBusHandler
    {
        Task SendMessagesAsync(string message, string sessionCode);
        void CloseConnection();
        void SetSubscriptionAsync(string connectionString, string topic, string subscriptionName, Func<IMessageSession, Message, CancellationToken, Task> onMessageRecivedCallBack);
    }
}