using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Database.Models
{
 public   class Product
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Cena { get; set; }
        [ForeignKey("ProductId")]
        public virtual List<DogovorProduct> DogovorProduct { get; set; }
        [ForeignKey("ProductId")]
        public virtual List<ProductSklad> ProductSklad { get; set; }

    }
}
