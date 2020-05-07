using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBus
{
    public interface IServiceBusTopicHandler
    {
        Task SendMessagesAsync(string message);
        Task CompleteMessageAsync(string lockToken);
    }
}