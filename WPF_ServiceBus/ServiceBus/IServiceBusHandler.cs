using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_ServiceBus.ServiceBus
{
    public interface IServiceBusHandler
    {
        Task SendMessagesAsync(string message);
        Task completeAsync(string lockToken);
    }
}