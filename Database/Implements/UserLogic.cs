using BusinessLogic.BindingModel;
using BusinessLogica.Interface;
using BusinessLogica.ViewModel;
using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Database.Implements
{
    public class UserLogic : IUser
    {
        public void CreateOrUpdate(UserBM model)
        {
            using (var context = new DatabaseContext())
            {
                User element = model.Id.HasValue ? null : new User();
                if (model.Id.HasValue)
                {
                    element = context.Users.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                }
                else
                {
                    element = new User();
                    context.Users.Add(element);
                }
                element.Login = model.Login;
                element.Password = model.Password;
                context.SaveChanges();
            }
        }
        public List<UserVM> Read(UserBM model)
        {
            using (var context = new DatabaseContext())
            {
                return context.Users
                 .Where(rec => model == null
                   || rec.Id == model.Id
                   || (rec.Login == model.Login && (rec.Password == model.Password || model.Password == null)))
               .Select(rec => new UserVM
               {
                   Id = rec.Id,
                   Login = rec.Login,
                   Password = rec.Password,

               })
                .ToList();
            }
        }
    }
 }


