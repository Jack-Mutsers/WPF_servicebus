using Newtonsoft.Json;
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
        private string _sessionCode { get; set; } = "AB12RB";

        public string SessionCode
        {
            get { return _sessionCode; }
            set { _sessionCode = value; }
        }

        public List<PlayerModel> playerList { get; set; } = new List<PlayerModel>();
        public PlayerModel self { get; set; }

        public ServiceBusHandler()
        {
            program = new Program();

            SessionData data = new SessionData();
            //queue connection string
            //data.connectionString = "Endpoint=sb://proftaak-test.servicebus.windows.net/;SharedAccessKeyName=chat;SharedAccessKey=J21bM387fclbAHaENUvHDH6KrI85aAGRtc9b/cGtLhY=";
            data.topic = "chat";
            data.subscription = Subscriptions.Join;

            data.queueName = "myfirstchat";

            program.SetData(data);
            //program.MessageReceived += OnMessageReceived;
        }

        public ServiceBusHandler(Program existingProgram)
        {
            program = existingProgram;
            //program.MessageReceived += OnMessageReceived;
        }

        public void ChangeSubscription(Subscriptions subscription)
        {
            program.UpdateSubscription(subscription);
        }

        public void SendMessage(string message, MessageType type, string sessionCode = "")
        {
            _sessionCode = sessionCode == "" ? _sessionCode : sessionCode;

            program.SendMessage(message, type, _sessionCode);
        }

        public void HandleMessage(string message)
        {
            TransferModel transfer = JsonConvert.DeserializeObject<TransferModel>(message);
            PlayerModel source;

            if (transfer.type == MessageType.JoinRequest)
            {
                source = JsonConvert.DeserializeObject<PlayerModel>(transfer.message);
                if (self.type == PlayerType.Host)
                {
                    int count = playerList.Count();
                    int exists = playerList.Where(p => p.userId == source.userId).Count();

                    if (count < 4 && exists == 0)
                    {
                        source.orderNumber = ++count;
                        playerList.Add(source);

                        SessionResponseModel response = new SessionResponseModel();
                        response.playerList = playerList;
                        response.Player = source;
                        response.accepted = true;

                        if (count == 1)
                            response.subscription = Subscriptions.ChannelOne;

                        if (count == 2)
                            response.subscription = Subscriptions.ChannelTwo;

                        if (count == 3)
                            response.subscription = Subscriptions.ChannelThree;

                        if (count == 4)
                            response.subscription = Subscriptions.ChannelFour;

                        string line = JsonConvert.SerializeObject(response);
                        SendMessage(line, MessageType.Response);
                    }

                }
            }

            if (transfer.type == MessageType.Response)
            {
                SessionResponseModel response = JsonConvert.DeserializeObject<SessionResponseModel>(transfer.message);
                PlayerModel player = response.Player;

                playerList = response.playerList;
                program.UpdateSubscription(response.subscription);

                if (player.userId == self.userId && player.name == self.name && player.type == self.type)
                    self = player;
            }

            int test = 1;
        }
    }
}
