using ServiceBus.Entities.Enums;
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
using ServiceBus.Resources;

namespace WPF_ServiceBus.Logics
{
    public class ServiceBusHandler
    {
        Dictionary<int, Subscriptions> subscription = new Dictionary<int, Subscriptions>()
        {
            { 1, Subscriptions.ChannelOne },
            { 2, Subscriptions.ChannelTwo },
            { 3, Subscriptions.ChannelThree },
            { 4, Subscriptions.ChannelFour },
        };

        public Program program { get; private set; }

        public ServiceBusHandler(Player player, bool host = false)
        {
            // create new instance of program
            program = new Program(player);

            // if player is host, create new topic to play the game in
            if (host)
            {
                // add host to the player list
                StaticResources.PlayerList.Add(StaticResources.user);

                // create new topic with subscriptions
                program.CreateNewTopic();
            }
        }

        public ServiceBusHandler(Program existingProgram)
        {
            // store instance of program with the session data for use
            program = existingProgram;
        }

        public void HandleQueueMessage(string message)
        {
            // check if player is identified
            if (StaticResources.user != null)
            {
                // decode message
                Transfer transfer = JsonConvert.DeserializeObject<Transfer>(message);
                Player source;

                // check if message type is JoinRequest, so we know how to decode the message inside the transfer object and we know how to use it
                if (transfer.type == MessageType.JoinRequest)
                {
                    //check if the player is the host, because only the host may handel messages of the type JoinRequest
                    if (StaticResources.user.type == PlayerType.Host)
                    {
                        // decode message
                        source = JsonConvert.DeserializeObject<Player>(transfer.message);

                        // count amount of people in the game
                        int playerCount = StaticResources.PlayerList.Count();

                        // check if the player is already in the game
                        int exists = StaticResources.PlayerList.Where(p => p.userId == source.userId).Count();

                        // check if there are less than 4 people in the game and the new request is from a new player
                        if (playerCount < 4 && exists == 0)
                        {
                            // set the player order, by increasing the playerCount before assigning it to the ordernumber
                            source.orderNumber = ++playerCount;

                            // add new player to the player list
                            List<Player> players = StaticResources.PlayerList;
                            players.Add(source);

                            // create response model
                            SessionResponse response = new SessionResponse();
                            response.Player = source;
                            response.accepted = true;
                            response.topicData = new TopicData()
                            {
                                TopicConnectionString = program.topic.TopicData.TopicConnectionString, // get newly created topic connection string
                                topic = program.topic.TopicData.topic, // get newly created topic name
                                subscription = subscription[playerCount] // assign subscription to the new player
                            };
                            
                            // convert the response model to a JsonString
                            string line = JsonConvert.SerializeObject(response);

                            // create writer program
                            program.CreateQueueWriter(PlayerType.Host, transfer.QueueData);

                            // send response data on the newly joined writer queue
                            program.QueueWriter.SendQueueMessage(line, MessageType.Response);

                            // disconnect from the writer queue
                            //program.QueueWriter.DisconnectFromQueue();

                            // create new player message, so everyone in the game can update their player list
                            NewPlayerMessage newPlayerMessage = new NewPlayerMessage();
                            newPlayerMessage.playerList = players;

                            // convert the NewPlayerMessage model to a JsonString
                            line = JsonConvert.SerializeObject(newPlayerMessage);

                            // send the new player message
                            program.topic.SendTopicMessage(line, MessageType.NewPlayer);
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
                    if (player.userId == StaticResources.user.userId && player.name == StaticResources.user.name && player.type == StaticResources.user.type)
                    {
                        // the response is for me
                        // update player data
                        StaticResources.user = player;

                        // store service bus topic data in program
                        program.CreateTopicConnection(response.topicData);

                        program.QueueWriter.DisconnectFromQueue();
                        program.QueueListner.DisconnectFromQueue();

                        program.DeleteListnerQueue();
                    }
                }
            }
        }

        public void HandleNewPlayerTopicMessage(string message)
        {
            // decode message
            Transfer transfer = JsonConvert.DeserializeObject<Transfer>(message);

            // check if the message is for a new player that has joined
            if (transfer.type == MessageType.NewPlayer)
            {
                // decode message to SessionResponseModel
                NewPlayerMessage response = JsonConvert.DeserializeObject<NewPlayerMessage>(transfer.message);

                // update the player collection with the newly joinend players
                StaticResources.PlayerList = response.playerList;
            }
        }
       
    }
}
