using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Database.Models
{
   public class User
    {
        public int? Id { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        [ForeignKey("UserId")]
        public virtual List<Dogovor> Dogovor { get; set; }
    }
}
