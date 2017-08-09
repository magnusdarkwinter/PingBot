using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using PingBot.Helpers;
using PingBot.Models;
using System.Threading.Tasks;

namespace PingBot
{
    [HubName("pingHub")]
    public class PingHub : Hub
    {
        private readonly Cron _cron;

        public PingHub() : this(Cron.Instance) { }

        public PingHub(Cron cron)
        {
            _cron = cron;
        }

        public IEnumerable<Client> GetAllClients()
        {
            return _cron.GetAllClients();
        }

        public void UpdateUI()
        {
            Clients.All.updateUI();
        }
    }
}