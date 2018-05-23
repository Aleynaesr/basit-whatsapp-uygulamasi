using AgProgramlamaOdev2.Models;
using AgProgramlamaOdev2.Models.DbModels;
using AgProgramlamaOdev2.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AgProgramlamaOdev2.Controllers
{
    public class AccountController : Controller
    {
        DatabaseContext databaseContext = new DatabaseContext();
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(RegisterLoginModel model)
        {
            User user = databaseContext.Users.Where(x => x.email == model.email && x.password == model.password).FirstOrDefault();
            if(user!=null)
            {
                Session["session"] = user.email;
            }
            else
            {

                TempData["Hata"] = "Email veya şifre hatalı";
                return View();
            }


            return RedirectToAction("Index","Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterLoginModel model)
        {
            databaseContext.Users.Add(new User()
            {
                email=model.email,
                password=model.password
            });

            databaseContext.SaveChanges();

            return RedirectToAction("Login");
        }

    }
}