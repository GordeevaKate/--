using BusinessLogic.BindingModel;
using BusinessLogica.ViewModel;
using System.Collections.Generic;
namespace BusinessLogica.Interface
{
        public interface ISklad
        {
            List<SkladVM> Read(SkladBM model);
            void CreateOrUpdate(SkladBM model);
        }
}
