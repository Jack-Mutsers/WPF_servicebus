using Database.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceBus.Entities.models
{
    public class SessionResponse
    {
        public List<Player> playerList { get; set; }
        public Player Player { get; set; }
        public Subscriptions subscription { get; set; }
        public bool accepted { get; set; } // status if you got in
    }
}
