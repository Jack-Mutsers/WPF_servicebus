using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.model
{
    public class PlayerModel
    {
        Guid userId { get; set; }
        string name { get; set; }
        int orderNumber { get; set; }

    }
}
