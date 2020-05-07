using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class PlayerForUpdateDto
    {
        public Guid userId { get; set; }
        public string name { get; set; }
        public int orderNumber { get; set; }
        public bool ready { get; set; }
        public PlayerType type { get; set; }
    }
}
