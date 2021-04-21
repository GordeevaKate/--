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
    public class SkladLogic : ISklad
    {
        public void CreateOrUpdate(SkladBM model)
        {
            using (var context = new DatabaseContext())
            {
                Sklad element = model.Id.HasValue ? null : new Sklad();
                if (model.Id.HasValue)
                {
                    element = context.Sklads.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                }
                else
                {
                    element = new Sklad();
                    context.Sklads.Add(element);
                }
                element.Name = model.Name;
                context.SaveChanges();
            }
        }

        public List<SkladVM> Read(SkladBM model)
        {
            using (var context = new DatabaseContext())
            {
                return context.Sklads
                 .Where(rec => model == null
                   || rec.Id == model.Id
                   || rec.Name == model.Name)
               .Select(rec => new SkladVM
               {
                   Id = rec.Id,
                   Name = rec.Name

               })
                .ToList();
            }
        }
    }
}
