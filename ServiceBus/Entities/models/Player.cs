using Database.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.Entities.models
{
    public class Player
    {
        public Guid userId { get; set; } = Guid.NewGuid();
        public string name { get; set; }
        public int orderNumber { get; set; } = 0;
        public bool ready { get; set; } = false;
        public PlayerType type { get; set; } = PlayerType.Guest;
    }

}
