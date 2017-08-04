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
            using (PingBotEntities context = new PingBotEntities())
            {
                NetTools nt = new NetTools();

                foreach(var client in context.Clients)
                {
                    bool result = nt.IsUp(client.IpAddress);

                    if(result != client.IsUp)
                    {
                        if(result == false)
                        {
                            string body = string.Format("PingBot says {0} {1} {2} is now down", client.Name, client.Provider, client.IpAddress);
                            nt.SendMail(body);
                        }
                        else
                        {
                            string body = string.Format("PingBot says {0} {1} {2} is now up", client.Name, client.Provider, client.IpAddress);
                            nt.SendMail(body);
                        }

                        client.IsUp = result;
                    }
                }
                context.SaveChanges();

                return View(context.Clients.ToList());
            }
        }
    }
}