﻿using ServiceBus.Entities.Enums;
using Newtonsoft.Json;
using ServiceBus;
using ServiceBus.Entities.models;
using ServiceBus.Data;
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
            get { return program.QueueData.sessionCode; }
        }

        private ObservableCollection<Player> playerCollection = new ObservableCollection<Player>();

        public ObservableCollection<Player> PlayerCollection
        {
            get { return playerCollection; }
            private set { playerCollection = value; }
        }

        public List<Player> PlayerList
        {
            get { return playerCollection.ToList(); }
        }

        public Player self 
        {
            get { return program.User; }
            set { program.User = value; }
        }

        public ServiceBusHandler()
        {
            // create new instance of program
            program = new Program();
        }

        public void CreateQueueConnection(string sessionCode, PlayerType playerType)
        {
            QueueData writerData;
            QueueData listnerData;

            if (playerType == PlayerType.Host)
            {
                // set connection data
                writerData = CreateHostQueueConnection(sessionCode, false);
                listnerData = CreateHostQueueConnection(sessionCode, true);
            }
            else
            {
                // set connection data
                writerData = CreateGuestQueueConnection(sessionCode, false);
                listnerData = CreateGuestQueueConnection(sessionCode, true);
            }

            // pass over connection data
            program.SetQueueData(writerData, listnerData);
        }

        private QueueData CreateHostQueueConnection(string sessionCode, bool reader)
        {
            // set connection data
            QueueData data = new QueueData();

            if (reader)
            {
                // topic connection string
                data.QueueConnectionString = "Endpoint=sb://fontysaquadis.servicebus.windows.net/;SharedAccessKeyName=listner;SharedAccessKey=OcHkmq27xlgQdRB1gMFJhbLNE7dmAYmzggr2ml/X+Go=;";
                data.queueName = "join";
                data.sessionCode = sessionCode;
            }
            else
            {
                // topic connection string
                data.QueueConnectionString = "Endpoint=sb://fontysaquadis.servicebus.windows.net/;SharedAccessKeyName=writer;SharedAccessKey=W/qSGSLJsyHbE7b1Hj3zW6fid3kB/S0izjixscZp5Yw=;";
                data.queueName = "response";
                data.sessionCode = sessionCode;
            }

            return data;
        }

        private QueueData CreateGuestQueueConnection(string sessionCode, bool reader)
        {
            // set connection data
            QueueData data = new QueueData();

            if (reader)
            {
                // topic connection string
                data.QueueConnectionString = "Endpoint=sb://fontysaquadis.servicebus.windows.net/;SharedAccessKeyName=listner;SharedAccessKey=GzBNcv3kFZ0p9kjodCafGOTVUKObvIMGX1msgtdwE4A=";
                data.queueName = "response";
                data.sessionCode = sessionCode;
            }
            else
            {
                // topic connection string
                data.QueueConnectionString = "Endpoint=sb://fontysaquadis.servicebus.windows.net/;SharedAccessKeyName=writer;SharedAccessKey=Qo3wBhYffRY/CSrpu4vcH7n+EtbVbRNZjC+HFEewb9c=;";
                data.queueName = "join";
                data.sessionCode = sessionCode;
            }

            return data;
        }

        public ServiceBusHandler(Program existingProgram)
        {
            // store instance of program with the session data for use
            program = existingProgram;
            program.SetTopicData();
        }

        public void SendQueueMessage(string message, MessageType type)
        {
            // sent requested message
            program.SendQueueMessage(message, type);
        }

        public void SendTopicMessage(string message, MessageType type)
        {
            // sent requested message
            program.SendTopicMessage(message, type);
        }

        public void HandleQueueMessage(string message)
        {
            // check if player is identified
            if (self != null)
            {
                // decode message
                Transfer transfer = JsonConvert.DeserializeObject<Transfer>(message);
                Player source;

                // check if message type is JoinRequest, so we know how to decode the message inside the transfer object and we know how to use it
                if (transfer.type == MessageType.JoinRequest)
                {
                    //check if the player is the host, because only the host may handel messages of the type JoinRequest
                    if (self.type == PlayerType.Host)
                    {
                        // decode message
                        source = JsonConvert.DeserializeObject<Player>(transfer.message);

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
                            List<Player> players = playerCollection.ToList();
                            players.Add(source);

                            // create response model
                            SessionResponse response = new SessionResponse();
                            response.Player = source;
                            response.accepted = true;
                            response.topicData = new TopicData()
                            {
                                TopicConnectionString = program.TopicData.TopicConnectionString, // get newly created topic connection string
                                topic = program.TopicData.topic // get newly created topic name
                            };

                            // check what channel will be assigned to the new player
                            if (playerCount == 1)
                                response.topicData.subscription = Subscriptions.ChannelOne;

                            if (playerCount == 2)
                                response.topicData.subscription = Subscriptions.ChannelTwo;

                            if (playerCount == 3)
                                response.topicData.subscription = Subscriptions.ChannelThree;

                            if (playerCount == 4)
                                response.topicData.subscription = Subscriptions.ChannelFour;

                            // convert the response model to a JsonString
                            string line = JsonConvert.SerializeObject(response);

                            // send response data
                            SendQueueMessage(line, MessageType.Response);

                            // create new player message, so everyone in the game can update their player list
                            NewPlayerMessage newPlayerMessage = new NewPlayerMessage();
                            newPlayerMessage.playerList = players;

                            // convert the NewPlayerMessage model to a JsonString
                            line = JsonConvert.SerializeObject(newPlayerMessage);

                            // send the new player message
                            SendTopicMessage(line, MessageType.NewPlayer);
                        }

                    }
                }

                // check if the message type is response
                if (transfer.type == MessageType.Response)
                {
                    // message type is response
                    // decode message to SessionResponseModel
                    SessionResponse response = JsonConvert.DeserializeObject<SessionResponse>(transfer.message);
                    
                    // get the player from the response model
                    Player player = response.Player;

                    // check if the response is meant for me
                    if (player.userId == self.userId && player.name == self.name && player.type == self.type)
                    {
                        // the response is for me
                        // update player data
                        self = player;

                        // store service bus topic data in program
                        program.StoreTopicData(response.topicData);
                    }
                }
            }
        }

        public void HandleTopicMessage(string message)
        {
            // decode message
            Transfer transfer = JsonConvert.DeserializeObject<Transfer>(message);

            if (transfer.type == MessageType.NewPlayer)
            {
                // decode message to SessionResponseModel
                NewPlayerMessage response = JsonConvert.DeserializeObject<NewPlayerMessage>(transfer.message);

                // update the player collection with the newly joinend players
                PlayerCollection = new ObservableCollection<Player>(response.playerList);
            }

        }

        public void SetHostData(Player player)
        {
            playerCollection.Add(player);
            self = player;
            program.CreateNewTopic();
        }
    }
}
