using BusinessLogic.BindingModel;
using BusinessLogica.Interface;
using BusinessLogica.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebClient.Models;
using System.Globalization;
using System.IO;
using BusinessLogica.HelperModels;
using Database.Implements;
using System.Runtime.Serialization.Json;
using BusinessLogica.ViewModel;
using System.IO.Compression;
using System;
using System.IO;
namespace WebClient.Controllers
{
    public class AdminController : Controller
    {
        private readonly ISklad sklad;
        private readonly IProduct product;
        private readonly IDogovor dogovor;
        private readonly IUser user;
        public AdminController(IProduct product, IUser user, IDogovor dogovor, ISklad sklad)
        {
            this.sklad = sklad;
            this.user = user;
            this.dogovor = dogovor;
            this.product = product;
        }
        public IActionResult Sklad()
        {
            if (TempData["ErrorLack"] != null)
            {
                ModelState.AddModelError("", TempData["ErrorLack"].ToString());
            }

            ViewBag.Sklad = sklad.Read(null);

            return View();
        }
        public ActionResult AddSklad(string _name)
        {
            if (_name == null)
            {
                TempData["ErrorLack"] = "Вы не ввели название";
                return RedirectToAction("Sklad");
            }
            var diagnosis = sklad.Read(new BusinessLogic.BindingModel.SkladBM { Name = _name });
            if (diagnosis.Count > 0)
            {
                TempData["ErrorLack"] = "Такая затрата уже есть в базе данных";
                return RedirectToAction("Sklad");
            }
            sklad.CreateOrUpdate(new BusinessLogic.BindingModel.SkladBM
            {

                Name = _name
            });
            return RedirectToAction("Sklad");
        }
        public IActionResult Product(int id)
        {
            Dictionary<int, decimal> count = new Dictionary<int, decimal>();
            if (TempData["ErrorLack"] != null)
            {
                ModelState.AddModelError("", TempData["ErrorLack"].ToString());
            }
            var Product = product.Read(null);
            foreach(var pro in Product)
            {
                     count.Add((int)pro.Id, product.ReadSP(new ProductSkladBM { SkladId = id, ProductId = (int)pro.Id }).Where(x => x.Status == BusinessLogica.Enum.Status.Пополнение).Sum(x => x.Count) -
                     product.ReadSP(new ProductSkladBM { SkladId = id, ProductId = (int)pro.Id }).Where(x => x.Status == BusinessLogica.Enum.Status.Списание).Sum(x => x.Count));

            }
            ViewBag.Sklad = sklad.Read( new SkladBM { Id=id}).FirstOrDefault();
            ViewBag.Count = count;
            ViewBag.Id = id;
            ViewBag.Product = Product;
            return View();
        }
        public IActionResult AddProduct( int id)
        {
            ViewBag.Id = id;
            return View();
        }
        [HttpPost]
        public ViewResult AddProduct(AddProductModel client, int id)
        {
            Dictionary<int, decimal> count = new Dictionary<int, decimal>();
            var Product = product.Read(null);
            ViewBag.Id = id;
            if (String.IsNullOrEmpty(client.Name))
            {
                ModelState.AddModelError("", "Введите логин");
                return View(client);
            }
            if (String.IsNullOrEmpty(client.Cena))
            {
                ModelState.AddModelError("", "Введите Cena");
                return View(client);
            }
            if ((decimal.TryParse(client.Cena, out decimal number)!=true)||(client.Cena.Length>8))
            {
                ModelState.AddModelError("", "В качестве цены должно быть указано число от 1 до 99999.99");
                return View(client);


            }
            string[] str = number.ToString(new System.Globalization.NumberFormatInfo() { NumberDecimalSeparator = "." }).Split('.');
            if ((str.Length == 2 ? str[1].Length : 0) > 2)
            {
                ModelState.AddModelError("", "В качестве цены должно быть указано число от 1 до 99999.99");
                return View(client);
            }
            if (String.IsNullOrEmpty(client.Count))
            {
                ModelState.AddModelError("", "Введите Count");
                return View(client);
            }
            if ((int.TryParse(client.Count, out int number1)!=true)||(client.Count.Length>5))
            {
                ModelState.AddModelError("", "В качестве количевство должно быть указано число от 1 до 99999");
                return View(client);
            }
            var existClient = product.Read(new ProductBM
            {
                Name = client.Name,
                Cena = Convert.ToDecimal(client.Cena)
            }).FirstOrDefault();
            if (existClient != null)
            {
                product.CreateSP(new ProductSkladBM
                {
                    ProductId = (int)existClient.Id,
                    SkladId = id,
                    Data = DateTime.Now,
                    Count =Convert.ToInt32( client.Count),
                    Status = Status.Пополнение
                }) ;
                foreach (var pro in Product)
                {
                    count.Add((int)pro.Id, product.ReadSP(new ProductSkladBM { SkladId = id, ProductId = (int)pro.Id }).Where(x => x.Status == BusinessLogica.Enum.Status.Пополнение).Sum(x => x.Count) -
                          product.ReadSP(new ProductSkladBM { SkladId = id, ProductId = (int)pro.Id }).Where(x => x.Status == BusinessLogica.Enum.Status.Списание).Sum(x => x.Count));

                }
                ViewBag.Sklad = sklad.Read(new SkladBM { Id = id }).FirstOrDefault();
                ViewBag.Count = count;
                ViewBag.Id = id;
                ViewBag.Product = Product;
                return View("Product", id);
            }
            product.CreateOrUpdate(new ProductBM
            {
                Name = client.Name,
                Cena = Convert.ToDecimal(client.Cena)
            });
             existClient = product.Read(new ProductBM
            {
                Name = client.Name,
                Cena = Convert.ToDecimal(client.Cena)
            }).FirstOrDefault();
            product.CreateSP(new ProductSkladBM
            {
                    ProductId = (int)existClient.Id,
                    SkladId = id,
                    Data = DateTime.Now,
                    Count = Convert.ToInt32(client.Count),
                    Status = Status.Пополнение
            });
             Product = product.Read(null);
            foreach (var pro in Product)
            {
                count.Add((int)pro.Id, product.ReadSP(new ProductSkladBM { SkladId = id, ProductId = (int)pro.Id }).Where(x => x.Status == BusinessLogica.Enum.Status.Пополнение).Sum(x => x.Count) -
                        product.ReadSP(new ProductSkladBM { SkladId = id, ProductId = (int)pro.Id }).Where(x => x.Status == BusinessLogica.Enum.Status.Списание).Sum(x => x.Count));

            }
            ViewBag.Sklad = sklad.Read(new SkladBM { Id = id }).FirstOrDefault();
            ViewBag.Count = count;
            ViewBag.Id = id;
            ViewBag.Product = Product;
            return View("Product", id);
        }
        public IActionResult Report()
        {
           


            return View();
        }
        public IActionResult Dogovors( )
        {
            ViewBag.Dogovor = dogovor.Read(null);
            ViewBag.User = user.Read(null);
            return View();
        }
        public IActionResult SendPuth(int id, DateTime date1, DateTime date2)
        {
            if (TempData["ErrorLack"] != null)
            {
                ModelState.AddModelError("", TempData["ErrorLack"].ToString());
            }
            if (id<0)
            if (date1 >= date2)
            {
                TempData["ErrorLack"] = "Дата 1 должна быть меньше даты 2";
                if (id == -1)
                {
                    ViewBag.Text = "Отчет по перемещения продукции";
                    ViewBag.Id = id;
                }
                else
                {
                    ViewBag.Text = "Отчет по работе сотрудников";
                    ViewBag.Id = id;
                }
                return RedirectToAction("ReportSklad");
            }
            if((date1 == new DateTime()) || (date2 == new DateTime())) { 
                TempData["ErrorLack"] = "Дата не должна равняться 01.01.0001";
            if (id == -1)
            {
                ViewBag.Text = "Отчет по перемещения продукции";
                ViewBag.Id = id;
            }
            else
            {
                ViewBag.Text = "Отчет по работе сотрудников";
                ViewBag.Id = id;
            }
            return RedirectToAction("ReportSklad");
            }
             ViewBag.Date1 = date1;
            ViewBag.Date2 = date2;
            ViewBag.Id = id;
            return View();
        }
        public IActionResult ReportSklad(int id)
        {
            if (TempData["ErrorLack"] != null)
            {
                ModelState.AddModelError("", TempData["ErrorLack"].ToString());
            }
            ViewBag.Text = "Archive";
            if (id == 0)
            {
                ViewBag.Id = -3;
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Validation(int Id, ReportViewModel model, DateTime date1, DateTime date2)
        {
            ViewBag.Id = Id;

            if (!Directory.Exists(model.Puth))
            {
                ViewBag.Date1 = date1;
                ViewBag.Date2 = date2;
                ViewBag.Id = Id;
                TempData["ErrorLack"] = "На данном компьютере нет такого пути";
                return RedirectToAction("SendPuth", new { date1=date1, date2=date2, id=Id});
            }
            if (String.IsNullOrEmpty(model.Name))
            {
                ViewBag.Date1 = date1;
                ViewBag.Date2 = date2;
                ViewBag.Id = Id;
                ModelState.AddModelError("", "Введите навзание");
                return RedirectToAction("SendPuth", new { date1 = date1, date2 = date2, id = Id });
            }
            if (Id == -3)
                {
                    Directory.CreateDirectory(model.Puth + model.Name);
                    if (Directory.Exists(model.Puth + model.Name))
                    {
                        var dogovors = dogovor.ReadDate(date1, date2);//все договоры
                        var oldproduct = dogovor.ReadP(new DogovorProductBM { Id = 0 });//устаревшие рейсы по договору
                        bool proverca = false;
                        foreach (var Dogovor in dogovors)
                        {
                            var product = dogovor.ReadP(new DogovorProductBM { DogovorId = (int)Dogovor.Id });

                            foreach (var dr in product)
                            {
                                oldproduct.Add(dogovor.ReadP(new DogovorProductBM { Id = dr.Id })[0]);

                            }
                            dogovor.Delete(new DogovorBM { Id = Dogovor.Id });

                        }
                        DataContractJsonSerializer jsonFormatter = new
                       DataContractJsonSerializer(typeof(List<DogovorVM>));
                        using (FileStream fs = new FileStream(string.Format("{0}/{1}.json",
                       model.Puth + model.Name, "Dogovors"), FileMode.OpenOrCreate))
                        {
                            jsonFormatter.WriteObject(fs, dogovors);
                        }
                        jsonFormatter = new
                     DataContractJsonSerializer(typeof(List<DogovorProductVM>));
                        using (FileStream fs = new FileStream(string.Format("{0}/{1}.json",
                     model.Puth + model.Name, "DogovorProductVM"), FileMode.OpenOrCreate))
                        {
                            jsonFormatter.WriteObject(fs, oldproduct);
                        }
                    System.IO.File.Delete($"{model.Puth + model.Name}.zip");
                        ZipFile.CreateFromDirectory(model.Puth + model.Name, $"{model.Puth + model.Name}.zip");
                       
                        Directory.Delete(model.Puth + model.Name, true);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        DirectoryInfo di = Directory.CreateDirectory(model.Puth + model.Name);
                        return RedirectToAction("Index", "Home");
                    }
                }


            if (Id > 0)
            {
                ReportLogic.CreateDoc(new PdfInfo
                {
                    id=1,
                    FileName = model.Puth + model.Name +".pdf",
                    Title = $"Договор № {Id}",
                    Products = product.Read(null),
                    DogovorProduct = dogovor.ReadP(new DogovorProductBM { DogovorId = Id }),
                    Dogovors = dogovor.Read(new DogovorBM { Id = Id }),
                    User = user.Read(new UserBM { Id = dogovor.Read(new DogovorBM { Id = Id })[0].UserId })

                });
                ViewBag.Dogovor = dogovor.Read(null);
                ViewBag.User = user.Read(null);
                return View("Dogovors");
              
            }
            if (Id == 0)
            {
                ReportLogic.CreateDoc(new PdfInfo
                {id=0,
                    FileName = model.Puth + model.Name + ".pdf",
                    Title = $"Все Договора",
                    Dogovors = dogovor.Read(null),
                    User = user.Read(null)
                });
                ViewBag.Dogovor = dogovor.Read(null);
                ViewBag.User = user.Read(null);
                return View("Dogovors");
            }
            if (Id == -1)
            {
                ReportLogic.CreateDoc(new PdfInfo
                {id=-1,
                    FileName = model.Puth + model.Name + ".pdf",
                    Title = $"Отчет по перемещения продукции по складам за {date1.ToShortDateString()}   {date2.ToShortDateString()}",
                    Sklad = sklad.Read(null),
                    Products = product.Read(null),
                    SkladProduct=product.ReadSPdate( date1, date2)
                });

                return View("Report");
            }
            if (Id == -2)
            {
                ReportLogic.CreateDoc(new PdfInfo
                {id=-2,
                    FileName = model.Puth + model.Name + ".pdf",
                    Title = $"Отчет по работе сотрудников за пeриод {date1.ToShortDateString()}   {date2.ToShortDateString()}",
                    Dogovors = dogovor.Read(null),
                    User = user.Read(null)
                });
                return View("Report");
            }
          
            return RedirectToAction("Index", "Home");
        }
        public IActionResult ReportReport(string Report)
        {
            if (Report == null)
            {
                ModelState.AddModelError("", "Выберете отчет");
                return View("Report");
            }
            if (Report== "Отчеты по договорам")
            {
                ViewBag.Dogovor = dogovor.Read(null);
                ViewBag.User = user.Read(null);
                return View("Dogovors");
            }
          else
            {
                ViewBag.Text = Report;
                if (Report == "Отчет по перемещения продукции")
                {
                    ViewBag.Id = -1;
                }
                else
                {
                    ViewBag.Id = -2;
                }
                return View("ReportSklad");
            }
        }
    }
}
