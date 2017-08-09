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
            // Settings and creating objects we will need
            int timeout = 6000;
            string data = "pingbotpingbotpingbotpingbotping";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions(64, true);
            
            // Perform Ping
            PingReply reply = pingSender.Send(ipaddress, timeout, buffer, options);

            // Check status and return true or false if ping is successful or not
            if (reply.Status == IPStatus.Success)
                return true;
       
            return false;
        }

        public void SendMail(string subject, string body)
        {
            // Create smtp object and change smtp settings
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("magnusdarkwinter@gmail.com", "rocawear6");

            // Create mail object and change mail settings
            MailMessage mail = new MailMessage("magnusdarkwinter@gmail.com", "travus@data-serv.com", subject, body);
            mail.BodyEncoding = UTF8Encoding.UTF8;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            
            // Send mail
            client.Send(mail);
        }
    }
}