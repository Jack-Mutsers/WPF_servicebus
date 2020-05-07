using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class PlayerForCreationDto
    {
        public string name { get; set; }
        public int orderNumber { get; set; }
        public bool ready { get; set; }
        public PlayerType type { get; set; }
    }
}
