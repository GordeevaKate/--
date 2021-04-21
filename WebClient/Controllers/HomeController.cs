using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebClient.Models;

namespace WebClient.Controllers
{
    public class HomeController : Controller
    {
      
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (TempData["ErrorLack"] != null)
            {
                ModelState.AddModelError("", TempData["ErrorLack"].ToString());
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Logout()
        {
            Program.User = null;
            Program.Admin = false;
            return RedirectToAction("Index", "Home");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
       
    }
}
