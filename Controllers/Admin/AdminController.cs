using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DataAccesLib.DataAccess;
using DataAccesLib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;


namespace VAII.Controllers
{
    public class AdminController : Controller
    {
        private const string salt = "J1CrH1hfeT";
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

            var foundUser = _dbContext.AdminsUsers.FirstOrDefault(u => u.UserName.Equals(user.UserName));
            if (foundUser == null)
            {
                
                //bad login
                TempData["msg"] = "Nesprávne meno alebo heslo";
                return RedirectToAction("Index", user);
            }
            else
            {
                var hashedPassword = foundUser.Password;

                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] saltBytes = new byte[36];
                rng.GetBytes(saltBytes);


                byte[] passwordAndSaltBytes = System.Text.Encoding.UTF8.GetBytes(user.Password + salt);
                byte[] hashBytes = new SHA256Managed().ComputeHash(passwordAndSaltBytes);
                string enteredHashedPassword = Convert.ToBase64String(hashBytes);

                if (enteredHashedPassword.Equals(hashedPassword))
                {
                    
                    HttpContext.Session.SetString("user", user.UserName);
                    TempData["user"] = user.UserName;
                    return RedirectToAction("Cms");
                }
                TempData["msg"] = "Nesprávne meno alebo heslo";
                return RedirectToAction("Index", user);
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
