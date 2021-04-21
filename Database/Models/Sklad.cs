using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Database.Models
{
  public  class Sklad
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        [ForeignKey("SkladId")]
        public virtual List<ProductSklad> ProductSklad { get; set; }

    }
}
