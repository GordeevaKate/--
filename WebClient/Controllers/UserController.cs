using BusinessLogica.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebClient.Models;

namespace WebClient.Controllers
{
    public class UserController : Controller
    {
        private readonly IUser user;
        public UserController(IUser user)
        {
            this.user = user;

        }

        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel client)
        {
            if (client.Login == "1" && client.Password == "admin")
            {
                Program.Admin = true;
                return RedirectToAction("Index", "Home");
            }
            var clientView = user.Read(new BusinessLogic.BindingModel.UserBM
            {
                Login = client.Login,
                Password = client.Password
            }).FirstOrDefault();

            if (clientView == null)
            {
                ModelState.AddModelError("", "Вы ввели неверный пароль, либо пользователь не найден");
                return View(client);
            }

            Program.User = clientView;

            return RedirectToAction("Index", "Home");
        }



        public IActionResult Registracia()
        {
            return View();
        }
        [HttpPost]
        public ViewResult Registracia(LoginModel client)
        {
            if (String.IsNullOrEmpty(client.Login))
            {
                ModelState.AddModelError("", "Введите логин");
                return View(client);
            }

            if (client.Login.Length > 50 ||
           client.Login.Length < 3)
            {
                ModelState.AddModelError("", $"Длина логина должна быть от {3} до {50} символов");
                return View(client);
            }
            var existClient = user.Read(new BusinessLogic.BindingModel.UserBM
            {
                Login = client.Login
            }).FirstOrDefault();
            if (existClient != null)
            {
                ModelState.AddModelError("", "Данный логин уже занят");
                return View(client);
            }
            if (Regex.IsMatch(client.Login, @"(\d*)", RegexOptions.IgnoreCase))
            {
                ModelState.AddModelError("", $"Логин должен состоять только из букв");
                return View(client);
            }

            if (client.Password.Length > 10 ||
            client.Password.Length < 6)
            {
                ModelState.AddModelError("", $"Длина пароля должна быть от {10} до {6} символов");
                return View(client);
            }

            if (String.IsNullOrEmpty(client.Password))
            {
                ModelState.AddModelError("", "Введите пароль");
                return View(client);
            }
           

            user.CreateOrUpdate(new BusinessLogic.BindingModel.UserBM
            {
                Login = client.Login,
                Password = client.Password
            });
            ModelState.AddModelError("", "Вы успешно зарегистрированы");
            return View("Registracia", client);
        }
    }
}
