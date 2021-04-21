using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using BusinessLogica.Enum;

namespace Database.Models
{
   public class ProductSklad
    {
        public int? Id { get; set; }
        public int SkladId { get; set; }
        public int ProductId { get; set; }
        [Required]
        public DateTime Data { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public Status Status { get; set; }
        public Sklad Sklad { get; set; }
        public Product Product { get; set; }
    }
}
