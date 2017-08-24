using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using PingBot.Models;
using PingBot.Helpers;

namespace PingBot.Helpers
{
    public class Cron
    {
        // Setup amount of time before running ping again.
        private readonly TimeSpan _updateInterval = TimeSpan.FromSeconds(5);
        private readonly Timer _timer;

        private IHubConnectionContext<dynamic> Clients { get; set; }

        private readonly static Lazy<Cron> _instance = 
            new Lazy<Cron>(() => new Cron(GlobalHost.ConnectionManager.GetHubContext<PingHub>().Clients));

        public static Cron Instance
        {
            get { return _instance.Value; }
        }

        private Cron(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
            _timer = new Timer(UpdatePingStatus, null, _updateInterval, _updateInterval);
        }

        public IEnumerable<Client> GetAllClients()
        {
            using (PingBotEntities context = new PingBotEntities())
            {
                return context.Clients.ToList();
            }
        }

        private void UpdatePingStatus(object state)
        {
            using (PingBotEntities context = new PingBotEntities())
            {
                NetTools nt = new NetTools();

                foreach (var client in context.Clients)
                {
                    // Run Ping
                    bool result = nt.IsUp(client.IpAddress);

                    // if something changed
                    if (result != client.IsUp)
                    {
                        if (result == false)
                        {
                            string subject = string.Format("PingBot says {0} is down", client.Name);
                            string body = string.Format("PingBot says {0} {1} {2} is now down", client.Name, client.Provider, client.IpAddress);
                            nt.SendMail(subject, body);
                        }
                        else
                        {
                            string subject = string.Format("PingBot says {0} is up", client.Name);
                            string body = string.Format("PingBot says {0} {1} {2} is now up", client.Name, client.Provider, client.IpAddress);
                            nt.SendMail(subject, body);
                        }

                        client.IsUp = result;
                        Clients.All.updatePingStatus(client); // brodcast changes to ui
                    }
                }
         
                context.SaveChanges(); // save changes to database
                
            }
        }
    }
}