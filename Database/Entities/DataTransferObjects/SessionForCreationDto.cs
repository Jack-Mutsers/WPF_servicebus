using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Entities.DataTransferObjects
{
    class SessionForCreationDto
    {
        public bool active { get; set; }
        public string session_code { get; set; }
    }
}
