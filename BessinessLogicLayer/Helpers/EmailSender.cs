using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BessinessLogicLayer.Helpers
{
    public class EmailSender:IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        private readonly SmtpClient _smtpClient;
        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings=emailSettings.Value;
            _smtpClient = new SmtpClient(_emailSettings.Server)
            {
                Port=_emailSettings.Port,
                EnableSsl=true,
                DeliveryMethod=SmtpDeliveryMethod.Network,
                UseDefaultCredentials=false
            };
            _smtpClient.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);

        }
        private MailMessage MailMessageServer(string from, string to, string displayName,string subject, string body)
        {
            var msg=new MailMessage(){ From=new MailAddress(from,displayName,Encoding.UTF8)};
            msg.To.Add(new MailAddress(to));
            msg.Subject=subject;
            msg.Body=body;
            msg.BodyEncoding=Encoding.UTF8;
            msg.IsBodyHtml=true;
            msg.Headers.Add("Mail", "App Mail");
            msg.Priority = MailPriority.High;
            msg.DeliveryNotificationOptions= DeliveryNotificationOptions.OnFailure;
            return msg;
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var msg = MailMessageServer(_emailSettings.Email, email, _emailSettings.SenderName, subject, htmlMessage);
            return _smtpClient.SendMailAsync(msg);
        }
    }
}
