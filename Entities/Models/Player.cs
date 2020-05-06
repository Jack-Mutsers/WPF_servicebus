using Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    [Table("player")]
    public class Player
    {
        public Guid userId { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "player name is required")]
        public string name { get; set; }
        public int orderNumber { get; set; } = 0;
        public PlayerType type { get; set; } = PlayerType.Guest;
    }
}
