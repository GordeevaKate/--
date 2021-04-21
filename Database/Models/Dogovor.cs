using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Database.Models
{
  public  class Dogovor
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        [Required]
        public decimal Cena { get; set; }
        [Required]
        public DateTime Data { get; set; }
        [Required]
        public string FIO { get; set; }
        [ForeignKey("DogovorId")]
        public virtual List<DogovorProduct> DogovorProduct { get; set; }
        public User User { get; set; }

    }
}
