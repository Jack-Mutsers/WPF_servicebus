using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_ServiceBus.ServiceBus.session
{
    public class SessionData
    {
        public string connectionString { get; set; } = "Endpoint=sb://proftaak-test.servicebus.windows.net/;SharedAccessKeyName=RootManagerSharedAccessKey;SharedAccessKey=HrPq1vOZCrhFGWycLEMhfQvYiP2Pu2GDuj2YawZ3bwU=;";
        public string topic { get; set; } = "chat";
        public string subscription { get; set; } = "S2DB04";
        public string queueName { get; set; } = "myfirstqueue";

        /*
         * subscriptions
         * 
         * Submarine,
         * SuperCool,
         * Hexinity,
         * S2DB04
         * 
         */
    }
}
