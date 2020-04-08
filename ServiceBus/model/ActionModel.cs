using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.model
{
    public class ActionModel
    {
        public Guid playerId { get; set; }
        public string sessionCode { get; set; }
        public CoordinatesModel coordinates { get; set; }
        public Action action { get; set; }

        public enum Action
        {
            shoot,
            surender
        }
    }

}
