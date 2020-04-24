using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServiceBus.Entities.models;
using ServiceBus.Data;
using System.Collections.Generic;
using System;
using ServiceBus.Entities.Enums;
using ServiceBus.Handlers;
using ServiceBus.Resources;

namespace ServiceBus
{
    public class QueueListnerHandler
    {
        private IServiceBusQueueHandler _ListnerQueueHandler;
        public QueueData QueueData { get; private set; }

        private SynchronizationContext _currentSynchronizationContext; // Needed to Synchronize between threads, Service buss handler is called from another thread

        public delegate void DataReceivedEventHandler(string source);
        public event DataReceivedEventHandler MessageReceived;

        public QueueListnerHandler(QueueData listnerData)
        {
            MessageReceived += DoNothing;
            _currentSynchronizationContext = SynchronizationContext.Current;

            // set the session data
            QueueData = listnerData;

            // assign handler
            _ListnerQueueHandler = new ServiceBusQueueHandler(listnerData.QueueConnectionString, listnerData.queueName, ProcessQueueSessionAsync);
        }

        private void DoNothing(string message){ } // this is required for the event, so we can use the event in another class

        public void setResponse(string message)
        {
            // trigger the MessageRecieved event, so another class can handle the newly recieved data
            MessageReceived(message);
        }

        public async Task ProcessQueueSessionAsync(IMessageSession messageSession, Message message, CancellationToken token)
        {
            // check if the message is for me by compairing the session code
            if (QueueData.sessionCode != messageSession.SessionId)
            {
                await Task.Yield();
                return;
            }

            // Process the message.
            string val = $"{Encoding.UTF8.GetString(message.Body)}";

            // check if the message is json encoded
            if (val.StartsWith("{") && val.EndsWith("}"))
            {

                // decode the json
                Transfer transfer = JsonConvert.DeserializeObject<Transfer>(val);

                // check if the transfer type of the response type is and check if the application is still on the join subscription
                if (transfer.type == MessageType.Response)
                {
                    // I am still waiting for my session response
                    // decode the response
                    var responseModel = JsonConvert.DeserializeObject<SessionResponse>(transfer.message);

                    // check if the response is meant for me
                    if (responseModel.Player.userId != StaticResources.user.userId)
                    {
                        // the response was not for me
                        await Task.Yield();
                        return;
                    }
                }

                // send message to the setResponse method
                _currentSynchronizationContext.Send(x => setResponse(val), null);
            }

            // complete the message so it is not recieved by anyone else
            await Task.CompletedTask;
        }

    }
}
