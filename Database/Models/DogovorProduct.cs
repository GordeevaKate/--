using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Database.Models
{
  public  class DogovorProduct
    {
        public int? Id { get; set; }
        public int DogovorId { get; set; }
        public int ProductId { get; set; }
        [Required]
        public int Count { get; set; }
        public Dogovor Dogovor { get; set; }
        public Product Product { get; set; }
    }
}
