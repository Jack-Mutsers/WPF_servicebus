using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.model
{
    public class SeesionModel
    {
        public string sessionCode { get; set; }
        public List<PlayerModel> players { get; set; }
        public Rights rights { get; set; }

    }

    public enum Rights
    {
        host,
        join
    }
}
