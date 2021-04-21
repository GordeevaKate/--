
using System;

namespace BusinessLogica.ViewModel
{
    public class DogovorVM
    {
          public int? Id { get; set; }
        public int UserId { get; set; }
        public decimal Cena { get; set; }
        public DateTime Data { get; set; }
        public string FIO { get; set; }
    }
}
