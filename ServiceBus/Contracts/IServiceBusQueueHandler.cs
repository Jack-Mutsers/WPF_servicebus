using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBus
{
    public interface IServiceBusQueueHandler
    {
        Task SendMessagesAsync(string message, string sessionCode);
        void DisconnectAsync();
    }
}