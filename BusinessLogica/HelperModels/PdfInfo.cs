using BusinessLogica.ViewModel;
using System.Collections.Generic;
namespace BusinessLogica.HelperModels
{
    public class PdfInfo
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public List<DogovorVM> Dogovors { get; set; }
        public List<DogovorProductVM> DogovorProduct { get; set; }
        public List<ProductVM> Products { get; set; }
        public List<UserVM> User { get; set; }
        public List<SkladVM> Sklad { get; set; }
        public int id { get; set; }
        public List<ProductSkladVM> SkladProduct { get; set; }
    }
}
