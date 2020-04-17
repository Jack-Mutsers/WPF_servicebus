using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Entities.Models
{
    public class Session
    {
        public int id { get; set; }
        public bool active { get; set; } = true;
        public string session_code { get; set; }
    }
}
