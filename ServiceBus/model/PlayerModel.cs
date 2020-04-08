using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.model
{
    public class PlayerModel
    {
        public Guid userId { get; set; } = Guid.NewGuid();
        public string name { get; set; }
        public int orderNumber { get; set; } = 0;
        public bool ready { get; set; } = false;
        public PlayerType type { get; set; } = PlayerType.Guest;
    }

    // define Player type to know what to do with the message
    public enum PlayerType
    {
        Host,
        Guest
    }
}
