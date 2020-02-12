using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using loanRV.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace loanRV.Controllers
{

    public class LogRegController : Controller
    {
        private RVContext dbContext;

        [HttpGet("logreg")]
        public IActionResult LogRegPage()
        {
            return View("LogReg");
        }

        [HttpPost]
        public IActionResult Register(LogRegViewModel model)
        {
            User user = model.User;

            if(ModelState.IsValid)
            {
                PasswordHasher<User> hasher = new PasswordHasher<User>();
                user.Password = hasher.HashPassword(user, user.Password);
                dbContext.Users.Add(user);
                dbContext.SaveChanges();
                Console.WriteLine("******** User Created ***********");
                HttpContext.Session.SetInt32("UserId", user.UserId);
                return RedirectToAction("Home", "Home");
            }
            return RedirectToAction("LogRegPage");
        }


        [HttpPost]
        public IActionResult Login(LogRegViewModel model)
        {
            if(ModelState.IsValid)
            {
                User user = dbContext.Users.FirstOrDefault(u => u.Email == model.Login.Email);
                if(user == null)
                {
                    ModelState.AddModelError("Email", "Incorrect password or email");
                    return Redirect("LogRegPage");
                }
                PasswordHasher<Login> hasher = new PasswordHasher<Login>();
                PasswordVerificationResult result = hasher.VerifyHashedPassword(model.Login, user.Password, model.Login.Password);

                if(result == 0)
                {
                    return RedirectToAction("LogRegPage");
                }
                else
                {
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    return RedirectToAction("Home", "Home");
                }
            }
            return RedirectToAction("LogRegPage");
        }

        public LogRegController(RVContext context) => dbContext = context;

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}