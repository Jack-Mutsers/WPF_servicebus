using Database.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entities.Models
{
    public class Player
    {
        Guid userId { get; set; } = Guid.NewGuid();
        string name { get; set; }
        int orderNumber { get; set; } = 0;
        bool ready { get; set; } = false;
        PlayerType type { get; set; } = PlayerType.Guest;
    }
}
