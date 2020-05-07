using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceBus.Entities.models
{
    public class NewPlayerMessage
    {
        public List<Player> playerList { get; set; }
    }
}
