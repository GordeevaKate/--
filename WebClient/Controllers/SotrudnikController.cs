using BusinessLogic.BindingModel;
using BusinessLogica.Enum;
using BusinessLogica.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebClient.Models;

namespace WebClient.Controllers
{
    public class SotrudnikController : Controller
    {
        private readonly ISklad sklad;
        private readonly IDogovor dogovor;
        private readonly IProduct product;
        public SotrudnikController(IDogovor dogovor, IProduct product, ISklad sklad)
        {
            this.dogovor = dogovor;
            this.sklad = sklad;
            this.product = product;
        }
        public IActionResult Dogovors()
        {
            ViewBag.Dogovor = dogovor.Read( new DogovorBM {UserId= (int)Program.User.Id ,Data=new DateTime() });
            return View();
        }
        private Dictionary<int, Dictionary<int, decimal>> CalculateSum()
        {
            var Sklad = sklad.Read(null);
            var Product = product.Read(null);
            Dictionary<int, Dictionary<int, decimal>> productsklad = new Dictionary<int, Dictionary<int, decimal>>();
            if (TempData["ErrorLack"] != null)
            {
                ModelState.AddModelError("", TempData["ErrorLack"].ToString());
            }
            foreach (var sklad in Sklad)
            {
                Dictionary<int, decimal> count = new Dictionary<int, decimal>();
                foreach (var pro in Product)
                {

                    count.Add((int)pro.Id, product.ReadSP(new BusinessLogic.BindingModel.ProductSkladBM { SkladId = (int)sklad.Id, ProductId = (int)pro.Id }).Where(x => x.Status == BusinessLogica.Enum.Status.Пополнение).Sum(x => x.Count) -
                    product.ReadSP(new BusinessLogic.BindingModel.ProductSkladBM { SkladId = (int)sklad.Id, ProductId = (int)pro.Id }).Where(x => x.Status == BusinessLogica.Enum.Status.Списание).Sum(x => x.Count));
                    if ((product.ReadSP(new BusinessLogic.BindingModel.ProductSkladBM { SkladId = (int)sklad.Id, ProductId = (int)pro.Id }).Where(x => x.Status == BusinessLogica.Enum.Status.Пополнение).Sum(x => x.Count) -
                        product.ReadSP(new BusinessLogic.BindingModel.ProductSkladBM { SkladId = (int)sklad.Id, ProductId = (int)pro.Id }).Where(x => x.Status == BusinessLogica.Enum.Status.Списание).Sum(x => x.Count)) > 0)
                    {
                    }
                }
                productsklad.Add((int)sklad.Id, count);

            }
            return productsklad;
        }
        public IActionResult CreateDogovor()
        {
            var Sklad = sklad.Read(null);
            var Product = product.Read(null);
            Dictionary<int, Dictionary<int, decimal>> productsklad = new Dictionary<int, Dictionary<int, decimal>>();
            if (TempData["ErrorLack"] != null)
            {
                ModelState.AddModelError("", TempData["ErrorLack"].ToString());
            }
            bool proverka = false;
            foreach(var sklad in Sklad)
            {
                Dictionary<int, decimal> count = new Dictionary<int, decimal>();
                foreach (var pro in Product)
                {
        
                        count.Add((int)pro.Id, product.ReadSP(new ProductSkladBM { SkladId = (int)sklad.Id, ProductId = (int)pro.Id }).Where(x => x.Status == BusinessLogica.Enum.Status.Пополнение).Sum(x => x.Count) -
                        product.ReadSP(new ProductSkladBM { SkladId = (int)sklad.Id, ProductId = (int)pro.Id }).Where(x => x.Status == BusinessLogica.Enum.Status.Списание).Sum(x => x.Count));
                    if((product.ReadSP(new ProductSkladBM { SkladId = (int)sklad.Id, ProductId = (int)pro.Id }).Where(x => x.Status == BusinessLogica.Enum.Status.Пополнение).Sum(x => x.Count) -
                        product.ReadSP(new ProductSkladBM { SkladId = (int)sklad.Id, ProductId = (int)pro.Id }).Where(x => x.Status == BusinessLogica.Enum.Status.Списание).Sum(x => x.Count)) > 0)
                    {
                        proverka = true;
                    }
                }
                productsklad.Add((int)sklad.Id, count);

            }
            if (!proverka)
            {
                ModelState.AddModelError("", "На всех складах отсутсвует продукция. Пожалуйста обратитесь к Администратору.");
                return View("Index");
            }
            ViewBag.Sklad = Sklad;

            ViewBag.PS = productsklad;
            ViewBag.Product = Product;
            return View();
        }
        [HttpPost]
        public ViewResult CreateDogovor(ProductDogovor model)
        {
            bool proverka = false;
            if (String.IsNullOrEmpty(model.Name))
            {
                ViewBag.Sklad = sklad.Read(null); ;

                ViewBag.PS = CalculateSum();
                ViewBag.Product = product.Read(null); ;
                ModelState.AddModelError("", "Введите логин");
                return View();
            }
            decimal sumcena = 0;
            foreach (var Count in model.ProductDogovors)
            {
                foreach (var count in Count.Value)
                {
                    if (Convert.ToInt32(count.Value) > 0)
                {
                    proverka = true;
                    sumcena = sumcena + product.Read(null).FirstOrDefault().Cena * Convert.ToInt32(count.Value);
                }
            }
            }
            if (!proverka)
            {
                ViewBag.Sklad = sklad.Read(null); ;
                ViewBag.PS = CalculateSum();
                ViewBag.Product = product.Read(null);
                ModelState.AddModelError("", "Вы не выбрали продукцию");
                return View();
            }
            var DateNov = DateTime.Now;
            dogovor.CreateOrUpdate(new DogovorBM {
                UserId = (int)Program.User.Id,
                Data = DateNov,
                FIO = model.Name,
                Cena = sumcena
            });

            foreach (var Count in model.ProductDogovors)
            {
                foreach (var count in Count.Value)
                {
                    if (Convert.ToInt32(count.Value) > 0)
                {
                    product.CreateSP(new ProductSkladBM {
                        SkladId = Count.Key,
                        ProductId = count.Key,
                        Count = Convert.ToInt32(count.Value),
                        Data = DateNov,
                        Status = Status.Списание,

                    });

                    dogovor.CreateP(new DogovorProductBM {
                        Count = Convert.ToInt32(count.Value),
                        ProductId = count.Key,
                        DogovorId = (int)dogovor.Read(new DogovorBM { UserId = (int)Program.User.Id, Data = DateNov })[0].Id

                    });
                }
            }
        }
            ViewBag.Dogovor = dogovor.Read(new DogovorBM { UserId = (int)Program.User.Id, Data = new DateTime() });

            return View("Dogovors");
        }
        public IActionResult Index()
        {
            if (TempData["ErrorLack"] != null)
            {
                ModelState.AddModelError("", TempData["ErrorLack"].ToString());
            }
            return View();
        }

    }
}
