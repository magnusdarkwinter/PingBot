using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PingBot.Models;
using PingBot.Helpers;

namespace PingBot.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            using (PingBotEntities1 context = new PingBotEntities1())
            {
                string username = form.Get("username");
                string password = form.Get("password");

                var checkUsername = context.Admins.Where(u => u.Username == username).FirstOrDefault();

                if (checkUsername != null)
                {
                    var knownPassword = checkUsername.Password;
                    var checkPassword = Security.CreateMD5(password);
                    if(checkPassword.ToLower() == knownPassword.ToLower())
                    {
                        return View("../Bot/Index");
                    }
                }
            }

            return View();
        }

    }
}