using ServiceBus.Entities.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceBus.Resources
{
    public static class StaticResources
    {
        public static Player user { get; set; }
        public static List<Player> PlayerList { get; set; } = new List<Player>();
        public static string sessionCode { get; set; }
    }
}
