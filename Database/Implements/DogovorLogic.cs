using BusinessLogic.BindingModel;
using BusinessLogica.Interface;
using BusinessLogica.ViewModel;
using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Database.Implements
{
    public class DogovorLogic : IDogovor
    {
        public void CreateOrUpdate(DogovorBM model)
        {
            using (var context = new DatabaseContext())
            {
                Dogovor element = model.Id.HasValue ? null : new Dogovor();
                if (model.Id.HasValue)
                {
                    element = context.Dogovors.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                }
                else
                {
                    element = new Dogovor();
                    context.Dogovors.Add(element);
                }
                element.FIO = model.FIO;
                element.Cena = model.Cena;
                element.Data = model.Data;
                element.UserId = model.UserId;
                context.SaveChanges();
            }
        }

        public void CreateP(DogovorProductBM model)
        {
            using (var context = new DatabaseContext())
            {
                DogovorProduct element = model.Id.HasValue ? null : new DogovorProduct();
                if (model.Id.HasValue)
                {
                    element = context.DogovorProducts.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                }
                else
                {
                    element = new DogovorProduct();
                    context.DogovorProducts.Add(element);
                }
                element.Count = model.Count;
                element.DogovorId = model.DogovorId;
                element.ProductId = model.ProductId;
                context.SaveChanges();
            }
        }

        public void Delete(DogovorBM dogovorBM)
        {
            using (var context = new  DatabaseContext())
            {
                Dogovor element = context.Dogovors.FirstOrDefault(rec => rec.Id ==  dogovorBM.Id);

                if (element != null)
                {
                    context.Dogovors.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        public List<DogovorVM> Read(DogovorBM model)
        {
            using (var context = new DatabaseContext())
            {
                return context.Dogovors
                 .Where(rec => model == null
                   || rec.Id == model.Id || (model.Data == new DateTime() && rec.UserId == model.UserId)
                   || (rec.Data == model.Data && rec.UserId == model.UserId))
               .Select(rec => new DogovorVM
               {
                   Id = rec.Id,
                   Data=rec.Data,
                   UserId=rec.UserId,
                   FIO=rec.FIO,
                   Cena=rec.Cena

               })
                .ToList();
            }
        }

        public List<DogovorVM> ReadDate(DateTime date1, DateTime date2)
        {
            using (var context = new DatabaseContext())
            {
                return context.Dogovors
                 .Where(rec =>
                    (rec.Data<=date2 && rec.Data>=date1))
               .Select(rec => new DogovorVM
               {
                   Id = rec.Id,
                   Data = rec.Data,
                   UserId = rec.UserId,
                   FIO = rec.FIO,
                   Cena = rec.Cena

               })
                .ToList();
            }
        }

        public List<DogovorProductVM> ReadP(DogovorProductBM model)
        {
            using (var context = new DatabaseContext())
            {
                return context.DogovorProducts
                 .Where(rec => model == null
                   || rec.Id == model.Id
                   || (rec.DogovorId == model.DogovorId))
               .Select(rec => new DogovorProductVM
               {
                   Id = rec.Id,
                    DogovorId=rec.DogovorId,
                     Count=rec.Count,
                     ProductId=rec.ProductId

               })
                .ToList();
            }
        }
    }
}
