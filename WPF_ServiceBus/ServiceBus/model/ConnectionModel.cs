using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_ServiceBus.ServiceBus.model
{
    public class ConnectionModel
    {
        public string naam { get; set; }
        public string sessionCode { get; set; }
        public Coordinates coordinates { get; set; }
        public Action action { get; set; }

        public enum Action
        {
            setBoat,
            joinSession,
            shoot,
            surender
        }


    }

}
