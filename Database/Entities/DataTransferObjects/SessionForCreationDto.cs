using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Entities.DataTransferObjects
{
    class SessionForCreationDto
    {
        bool active { get; set; }
        string session_code { get; set; }
    }
}
