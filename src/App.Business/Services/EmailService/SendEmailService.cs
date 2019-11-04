using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace App.Business.Services.EmailService
{
    public class SendEmailService : ISendEmailService
    {
        private readonly IConfiguration _config;

        public SendEmailService(IConfiguration configuration)
        {
            _config = configuration;
        }

        public Task SendAsync(List<string> recipientList, string body, string subject = null)
        {
            using (var msg = new MailMessage())
            {
                // Довільна кількість адресатів у List
                if ((recipientList != null) && (recipientList.Count > 0))
                {
                    foreach (var item in recipientList)
                    {
                        msg.To.Add(item);
                    }
                }
                else
                {
                    throw new Exception("Відсутні адресати для відправки листа");
                    return Task.FromResult(0);
                }

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
