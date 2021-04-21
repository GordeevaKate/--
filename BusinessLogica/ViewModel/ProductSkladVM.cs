using BusinessLogica.Enum;
using System;
namespace BusinessLogica.ViewModel
{
    public class ProductSkladVM
    {

        public int? Id { get; set; }
        public int SkladId { get; set; }
        public int ProductId { get; set; }
        public DateTime Data { get; set; }
        public int Count { get; set; }
        public Status Status { get; set; }
    }
}
