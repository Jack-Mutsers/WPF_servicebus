using Database.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Entities.DataTransferObjects
{
    class PlayerForUpdateDto
    {
        Guid userId { get; set; }
        string name { get; set; }
        int orderNumber { get; set; }
        bool ready { get; set; }
        PlayerType type { get; set; }
    }
}
