using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebClient.Models
{
    public class AddProductModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Cena { get; set; }
        [Required]
        public string Count { get; set; }
    }
}
