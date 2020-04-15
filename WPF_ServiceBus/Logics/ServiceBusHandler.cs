using Newtonsoft.Json;
using ServiceBus;
using ServiceBus.model;
using ServiceBus.session;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_ServiceBus.Logics
{
    public class ServiceBusHandler
    {
        public Program program { get; private set; }

        public string SessionCode
        {
            get { return program.SessionData.sessionCode; }
        }

        private ObservableCollection<PlayerModel> playerCollection = new ObservableCollection<PlayerModel>();

        public ObservableCollection<PlayerModel> PlayerCollection
        {
            get { return playerCollection; }
            private set { playerCollection = value; }
        }

        public List<PlayerModel> PlayerList
        {
            get { return playerCollection.ToList(); }
        }

        public PlayerModel self 
        {
            get { return program.User; }
            set { program.User = value; }
        }

        public ServiceBusHandler(string sessionCode)
        {
            // create new instance of program
            program = new Program();

            // set connection data
            SessionData data = new SessionData();

            // topic connection string
            data.connectionString = "Endpoint=sb://fontysaquadis.servicebus.windows.net/;SharedAccessKeyName=AccessManagement;SharedAccessKey=7fPwUZb0t5nxmd15min/ubFom/yGK5ryf9or31tdjog=;";
            data.topic = "chat";
            data.subscription = Subscriptions.Join;
            data.queueName = "myfirstchat";
            data.sessionCode = sessionCode;

            // pass over connection data
            program.SetData(data);
        }

        public ServiceBusHandler(Program existingProgram)
        {
            // store instance of program with the session data for use
            program = existingProgram;
        }

        public void ChangeSubscription(Subscriptions subscription)
        {
            // change to the assigned subscription
            program.UpdateSubscription(subscription);
        }

        public void SendMessage(string message, MessageType type)
        {
            // sent requested message
            program.SendMessage(message, type);
        }

        public void HandleMessage(string message)
        {
            // check if player is identified
            if (self != null)
            {
                // decode message
                TransferModel transfer = JsonConvert.DeserializeObject<TransferModel>(message);
                PlayerModel source;

                // check if message type is JoinRequest, so we know how to decode the message inside the transfer object and we know how to use it
                if (transfer.type == MessageType.JoinRequest)
                {
                    //check if the player is the host, because only the host may handel messages of the type JoinRequest
                    if (self.type == PlayerType.Host)
                    {
                        // decode message
                        source = JsonConvert.DeserializeObject<PlayerModel>(transfer.message);

                        // count amount of people in the game
                        int playerCount = PlayerList.Count();

                        // check if the player is already in the game
                        int exists = PlayerList.Where(p => p.userId == source.userId).Count();

                        // check if there are less than 4 people in the game and the new request is from a new player
                        if (playerCount < 4 && exists == 0)
                        {
                            // set the player order, by increasing the playerCount before assigning it to the ordernumber
                            source.orderNumber = ++playerCount;

                            // add new player to the player list
                            List<PlayerModel> players = playerCollection.ToList();
                            players.Add(source);

                            // create response model
                            SessionResponseModel response = new SessionResponseModel();
                            response.playerList = players;
                            response.Player = source;
                            response.accepted = true;

                            // check what channel will be assigned to the new player
                            if (playerCount == 1)
                                response.subscription = Subscriptions.ChannelOne;

                            if (playerCount == 2)
                                response.subscription = Subscriptions.ChannelTwo;

                            if (playerCount == 3)
                                response.subscription = Subscriptions.ChannelThree;

                            if (playerCount == 4)
                                response.subscription = Subscriptions.ChannelFour;

                            // convert the response model to a JsonString
                            string line = JsonConvert.SerializeObject(response);

                            // send response data
                            SendMessage(line, MessageType.Response);
                        }

                    }
                }

                // check if the message type is response
                if (transfer.type == MessageType.Response)
                {
                    // message type is response
                    // decode message to SessionResponseModel
                    SessionResponseModel response = JsonConvert.DeserializeObject<SessionResponseModel>(transfer.message);
                    
                    // get the player from the response model
                    PlayerModel player = response.Player;

                    // update the player collection with the newly joinend players
                    PlayerCollection = new ObservableCollection<PlayerModel>(response.playerList);

                    // check if the response is meant for me
                    if (player.userId == self.userId && player.name == self.name && player.type == self.type)
                    {
                        // the response is for me
                        // update player data
                        self = player;

                        // change to the correct subscription
                        ChangeSubscription(response.subscription);
                    }
                }
            }
        }
    }
}
