using ServiceBus.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceBus.Data
{
    public class QueueTypes
    {
        public QueueData GetHostQueueListner()
        {
            // set connection data
            QueueData data = new QueueData()
            {
                QueueConnectionString = "Endpoint=sb://fontysaquadis.servicebus.windows.net/;SharedAccessKeyName=listner;SharedAccessKey=OcHkmq27xlgQdRB1gMFJhbLNE7dmAYmzggr2ml/X+Go=;",
                queueName = "join",
                sessionCode = StaticResources.sessionCode
            };

            return data;
        }

        public QueueData GetGuestQueueListner()
        {
            // set connection data
            QueueData data = new QueueData()
            {
                QueueConnectionString = "Endpoint=sb://fontysaquadis.servicebus.windows.net/;SharedAccessKeyName=listner;SharedAccessKey=GzBNcv3kFZ0p9kjodCafGOTVUKObvIMGX1msgtdwE4A=",
                queueName = "response",
                sessionCode = StaticResources.sessionCode
            };

            return data;
        }

        public QueueData GetHostQueueWriter()
        {
            // set connection data
            QueueData data = new QueueData(){
                QueueConnectionString = "Endpoint=sb://fontysaquadis.servicebus.windows.net/;SharedAccessKeyName=writer;SharedAccessKey=W/qSGSLJsyHbE7b1Hj3zW6fid3kB/S0izjixscZp5Yw=;",
                queueName = "response",
                sessionCode = StaticResources.sessionCode
            };

            return data;
        }

        public QueueData GetGuestQueueWriter()
        {
            // set connection data
            QueueData data = new QueueData()
            {
                QueueConnectionString = "Endpoint=sb://fontysaquadis.servicebus.windows.net/;SharedAccessKeyName=writer;SharedAccessKey=Qo3wBhYffRY/CSrpu4vcH7n+EtbVbRNZjC+HFEewb9c=;",
                queueName = "join",
                sessionCode = StaticResources.sessionCode
            };

            return data;
        }

    }
}
