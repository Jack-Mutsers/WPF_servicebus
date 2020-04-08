using ServiceBus.session;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceBus.model
{
    public class SessionResponseModel
    {
        public List<PlayerModel> playerList { get; set; }
        public PlayerModel Player { get; set; }
        public Subscriptions subscription { get; set; }
        public bool accepted { get; set; } // status if you got in
    }
}
