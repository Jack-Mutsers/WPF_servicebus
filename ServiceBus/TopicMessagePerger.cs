using Microsoft.Azure.ServiceBus;
using ServiceBus.session;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceBus
{
    class TopicMessagePerger
    {
        //static void PurgeMessagesFromSubscription(SessionData data)
        //{
        //    // convert subscription emum to string
        //    string subscription = Enum.GetName(typeof(Subscriptions), data.subscription);
        //    int batchSize = 100;
        //    SubscriptionClient subscriptionClient = new SubscriptionClient(data.connectionString, data.topic, subscription, ReceiveMode.ReceiveAndDelete);

        //    do
        //    {
        //        var messages = subscriptionClient.ReceiveBatch(batchSize);
        //        if (messages.Count() == 0)
        //        {
        //            break;
        //        }
        //    }
        //    while (true);
        //}
    }
}
