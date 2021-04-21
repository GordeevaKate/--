using BusinessLogic.BindingModel;
using BusinessLogica.ViewModel;
using System;
using System.Collections.Generic;
namespace BusinessLogica.Interface
{

        public interface IProduct
        {
            List<ProductVM> Read(ProductBM model);
            void CreateOrUpdate(ProductBM model);
        List<ProductSkladVM> ReadSP(ProductSkladBM model);
        List<ProductSkladVM> ReadSPdate(DateTime date1, DateTime date2);
        void CreateSP(ProductSkladBM model);
    }
}
