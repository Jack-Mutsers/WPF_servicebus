using Database.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Entities.DataTransferObjects
{
    class PlayerDto
    {
        public Guid userId { get; set; }
        public string name { get; set; }
        public int orderNumber { get; set; }
        public bool ready { get; set; }
        public PlayerType type { get; set; }
    }
}
