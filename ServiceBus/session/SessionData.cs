﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.session
{
    public class SessionData
    {
        public string connectionString { get; set; } = "Endpoint=sb://proftaak-test.servicebus.windows.net/;SharedAccessKeyName=RootManagerSharedAccessKey;SharedAccessKey=HrPq1vOZCrhFGWycLEMhfQvYiP2Pu2GDuj2YawZ3bwU=;";
        public string topic { get; set; } = "chat";
        public Subscriptions subscription { get; set; } = Subscriptions.ChannalOne;
        public string queueName { get; set; } = "myfirstqueue";
        
    }

    public enum Subscriptions
    {
        ChannalOne,
        ChannelTwo,
        ChannelThree,
        Channelfour,
        Join 
    }
}