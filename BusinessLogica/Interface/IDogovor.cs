using BusinessLogic.BindingModel;
using BusinessLogica.ViewModel;
using System;
using System.Collections.Generic;
namespace BusinessLogica.Interface
{
    public interface IDogovor
    {
        List<DogovorVM> Read(DogovorBM model);
        List<DogovorVM> ReadDate(DateTime date1, DateTime date2);
        void CreateOrUpdate(DogovorBM model);
        List<DogovorProductVM> ReadP(DogovorProductBM model);
        void CreateP(DogovorProductBM model);
        void Delete(DogovorBM dogovorBM);
    }
}
