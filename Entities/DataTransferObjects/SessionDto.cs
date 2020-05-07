using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class SessionDto
    {
        public int id { get; set; }
        public bool active { get; set; }
        public string session_code { get; set; }
    }
}
