using System.Collections.Generic;
using BusinessLogic.BindingModel;
using BusinessLogica.ViewModel;

namespace BusinessLogica.Interface
{
    public interface IUser
    {
        List<UserVM> Read(UserBM model);
        void CreateOrUpdate(UserBM model);

    }
}
