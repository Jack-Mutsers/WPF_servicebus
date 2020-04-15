using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus
{
    public interface IServiceBusHandler
    {
        Task SendMessagesAsync(string message, string sessionCode);
        void CloseConnection();
        //Task completeAsync(string lockToken);
    }
}