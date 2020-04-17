using Database.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.Entities.models
{
    public class GameAction
    {
        public Guid playerId { get; set; }
        public string sessionCode { get; set; }
        public Coordinates coordinates { get; set; }
        public PlayerAction action { get; set; }

    }

}
