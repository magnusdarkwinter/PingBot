using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Mail;
using System.Text;
using System.Threading;

namespace PingBot.Helpers
{
    public class NetTools
    {
        public bool IsUp(string ipaddress)
        {
            int timeout = 6000;
            string data = "pingbotpingbotpingbotpingbotping";
            byte[] buffer = Encoding.ASCII.GetBytes(data);

            Ping pingSender = new Ping();
            PingOptions options = new PingOptions(64, true);
            
            PingReply reply = pingSender.Send(ipaddress, timeout, buffer, options);

            if (reply.Status == IPStatus.Success)
                return true;
       
            return false;
        }

        public void SendMail(string body, string subject)
        {
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("magnusdarkwinter@gmail.com", "rocawear6");

            MailMessage mail = new MailMessage("magnusdarkwinter@gmail.com", "travus@data-serv.com", subject, body);
            mail.BodyEncoding = UTF8Encoding.UTF8;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            
            client.Send(mail);
        }






        public void PingTaskAsync(string ipaddress)
        {
            int timeout = 6000; // 6 seconds
            string data = "pingbotpingbotpingbotpingbotping";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            AutoResetEvent waiter = new AutoResetEvent(false);
            PingOptions options = new PingOptions(64, true);

            Ping pingSender = new Ping();
            pingSender.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);

            pingSender.SendAsync(ipaddress, timeout, buffer, options, waiter);
            waiter.WaitOne();
        }

        public void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                ((AutoResetEvent)e.UserState).Set();
            }
            else if (e.Error != null)
            {
                ((AutoResetEvent)e.UserState).Set();
            }
            else
            {
                PingReply reply = e.Reply;
                DisplayReply(reply);
                ((AutoResetEvent)e.UserState).Set();
            }
        }

        public void DisplayReply(PingReply reply)
        {
            if (reply == null)
                return;

            Console.WriteLine("ping status: {0}", reply.Status);
            if (reply.Status == IPStatus.Success)
            {
                Console.WriteLine("Address: {0}", reply.Address.ToString());
                Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
                Console.WriteLine("Time to live: {0}", reply.Options.Ttl);
                Console.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
                Console.WriteLine("Buffer size: {0}", reply.Buffer.Length);
            }
        }
    }
}