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
    public class ProductLogic : IProduct
    {
        public void CreateOrUpdate(ProductBM model)
        {
            using (var context = new DatabaseContext())
            {
                Product element = model.Id.HasValue ? null : new Product();
                if (model.Id.HasValue)
                {
                    element = context.Products.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                }
                else
                {
                    element = new Product();
                    context.Products.Add(element);
                }
                element.Name = model.Name;
                element.Cena = model.Cena;
                context.SaveChanges();
            }
        }

        public void CreateSP(ProductSkladBM model)
        {
            using (var context = new DatabaseContext())
            {
                ProductSklad element = model.Id.HasValue ? null : new ProductSklad();
                if (model.Id.HasValue)
                {
                    element = context.ProductSklads.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                }
                else
                {
                    element = new ProductSklad();
                    context.ProductSklads.Add(element);
                }
                element.Count = model.Count;
                element.Data = model.Data;
                element.Status = model.Status;
                element.SkladId = model.SkladId;
                element.ProductId = model.ProductId;
                context.SaveChanges();
            }
        }

        public List<ProductVM> Read(ProductBM model)
        {
            using (var context = new DatabaseContext())
            {
                return context.Products
                 .Where(rec => model == null
                   || rec.Id == model.Id
                   || (rec.Name == model.Name &&rec.Cena==model.Cena) )
               .Select(rec => new ProductVM
               {
                   Id = rec.Id,
                   Name = rec.Name,
                   Cena=rec.Cena

               })
                .ToList();
            }
        }

        public List<ProductSkladVM> ReadSP(ProductSkladBM model)
        {
            using (var context = new DatabaseContext())
            {
                return context.ProductSklads
                 .Where(rec => model == null
                   || rec.Id == model.Id
                   ||(rec.SkladId == model.SkladId && rec.ProductId == model.ProductId )|| (rec.SkladId == model.SkladId && model.ProductId==0 ))
               .Select(rec => new ProductSkladVM
               {
                   Id = rec.Id,
                   Status=rec.Status,
                    SkladId=rec.SkladId,
                    ProductId=rec.ProductId,
                     Data=rec.Data,
                      Count=rec.Count
               })
                .ToList();
            }
        }

        public List<ProductSkladVM> ReadSPdate(DateTime date1, DateTime date2)
        {
            using (var context = new DatabaseContext())
            {
                return context.ProductSklads
                 .Where(rec => 
                  
                   (rec.Data<=date2  && rec.Data>=date1))
               .Select(rec => new ProductSkladVM
               {
                   Id = rec.Id,
                   Status = rec.Status,
                   SkladId = rec.SkladId,
                   ProductId = rec.ProductId,
                   Data = rec.Data,
                   Count = rec.Count
               })
                .ToList();
            }
        }
    }
}
