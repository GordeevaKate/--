using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebClient.Models
{
    public class ProductDogovor
    {
        public string Name { get; set; }
        public Dictionary<int, Dictionary<int, decimal>> ProductDogovors { get; set; }
    }
}
