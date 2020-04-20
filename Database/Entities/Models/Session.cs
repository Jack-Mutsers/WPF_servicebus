using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Database.Entities.Models
{
    [Table("session")]
    public class Session
    {
        public int id { get; set; }
        public bool active { get; set; } = true;

        [Required(ErrorMessage = "session code is required")]
        public string session_code { get; set; }
    }
}
