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
using Api;

namespace WPF_ServiceBus.Logics
{
    public class ServiceBusHandler
    {
        public Program program { get; private set; }

        public ServiceBusHandler(Player player)
        {
            // create new instance of program
            StaticResources.user = player;

        }

        public void StartHost()
        {
            ApiResponse response = ApiConnector.CreateHost();
            
            program = new Program(response.topicData);
            
            StaticResources.PlayerList = response.players.ToList();
            
            StaticResources.sessionCode = response.session_code;
        }

        public void JoinHost()
        {
            ApiResponse response = ApiConnector.JoinHost();
            
            program = new Program(response.topicData);

            StaticResources.PlayerList = response.players.ToList();

            NewPlayerMessage newPlayerMessage = new NewPlayerMessage()
            {
                playerList = StaticResources.PlayerList
            };

            string line = JsonConvert.SerializeObject(newPlayerMessage);

            program.SendTopicMessage(line, MessageType.NewPlayer);
        }

        public ServiceBusHandler(Program existingProgram)
        {
            // store instance of program with the session data for use
            program = existingProgram;
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
                StaticResources.PlayerList = response.playerList;
            }
        }
       
    }
}
