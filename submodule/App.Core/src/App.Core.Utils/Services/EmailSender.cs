using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using App.Core.Utils.Settings.Configuration;
using Microsoft.Extensions.Configuration;

namespace App.Core.Utils.Services
{
    public interface IEmailSender
    {
        Task SendAsync(string recipientName, string body, string subject = null);
    }

    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        public EmailSender(IConfiguration configuration)
        {
            _config = configuration;
        }


        public Task SendAsync(string recipientName, string body, string subject = null)
        {
            using (var msg = new MailMessage())
            {
                msg.To.Add(recipientName);
                msg.From = new MailAddress(_config["SMTPSettings:MailAddress"]);
                msg.Subject = subject ?? "Центр реабілітації дітей-інвалідів";
                msg.Body = body; msg.SubjectEncoding = System.Text.Encoding.UTF8;
                msg.BodyEncoding = System.Text.Encoding.UTF8;
                msg.IsBodyHtml = true; msg.Priority = MailPriority.Normal;

                using (var client = new SmtpClient())
                {
#if DEBUG
                    client.Host = _config["SMTPSettings:SMTPClientHost"];
#else
                    client.Host = _config["SMTPSettings:SMTPClientHost"];
#endif
                    client.EnableSsl = true;
                    client.Port = 587;
                    client.Credentials = new NetworkCredential(_config["SMTPSettings:UserName"], _config["SMTPSettings:Password"]);
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    try
                    {
                        client.Send(msg);
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            return Task.FromResult(0);
        }
    }

}
