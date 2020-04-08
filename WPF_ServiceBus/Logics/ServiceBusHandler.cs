using ServiceBus;
using ServiceBus.model;
using ServiceBus.session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_ServiceBus.Logics
{
    public class ServiceBusHandler
    {
        public Program program { get; private set; }

        public ServiceBusHandler(bool host)
        {
            program = new Program();

            SessionData data = new SessionData();
            data.connectionString = "Endpoint=sb://proftaak-test.servicebus.windows.net/;SharedAccessKeyName=chat;SharedAccessKey=J21bM387fclbAHaENUvHDH6KrI85aAGRtc9b/cGtLhY=";
            data.topic = "Chat";

            if (host)
            {
                data.subscription = Subscriptions.ChannalOne;
            }
            else
            {
                data.subscription = Subscriptions.Join;
            }

            data.queueName = "myfirstchat";

            program.SetData(data);
            program.MessageReceived += OnMessageReceived;
        }

        public ServiceBusHandler(Program existingProgram)
        {
            program = existingProgram;
            program.MessageReceived += OnMessageReceived;
        }

        public void ChangeSubscription(Subscriptions subscription)
        {
            program.UpdateSubscription(subscription);
        }

        public void SendMessage(ActionModel actionModel)
        {
            program.SendMessage(actionModel);
        }

        public void OnMessageReceived(ActionModel source)
        {
            // here comes the code execution on incomming message
        }
    }
}
