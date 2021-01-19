using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using DataAccesLib.DataAccess;
using DataAccesLib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;


namespace VAII.Controllers
{
    public class AdminController : Controller
    {
        private DataContext _dbContext;
       // private SignInManager<AdminUser> _signInManager;
        public AdminController(DataContext dbContext)
        {
            
            _dbContext = dbContext;
        }

        public ActionResult Index()
        {
            if (IsLoggedIn())
            {
                return RedirectToAction("Cms");
            } 
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Authorise([Bind("UserName,Password")] AdminUser user)
        {
            if (!ModelState.IsValid) return View("Index",user);
            var foundUser = _dbContext.AdminsUsers.FirstOrDefault(u => u.UserName.Equals(user.UserName) && u.Password.Equals(user.Password));
            if (foundUser == null)
            {
                //bad login
                TempData["msg"] = "Nesprávne meno alebo heslo";
                return RedirectToAction("Index", user);
            }
            else
            {
                HttpContext.Session.SetString("user", user.UserName);
                TempData["user"] = user.UserName;
                return RedirectToAction("Cms");
            }




        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("user");
            return RedirectToAction("Index");
        }

        public IActionResult Cms()
        {
            if (IsLoggedIn())
            {
                return View();
            }
            
            return RedirectToAction("Index");


        }

        public IActionResult Servis()
        {
            return  View();
        }

        public IActionResult SmartNews()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Kontakt()
        {
            return View();
        }

        private bool IsLoggedIn()
        {
            
            if (HttpContext.Session.TryGetValue("user", out var name))
            {
                TempData["user"] = System.Text.Encoding.Default.GetString(name);
                return true;
            }

            RedirectToAction("Index", "Admin");
            return false;

        }
    }
}
