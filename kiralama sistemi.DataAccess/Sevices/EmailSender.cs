using kiralamaSistemi.Entities.Abstract;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;

namespace kiralamaSistemi.DataAccess.Sevices
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MailMessage mm = new MailMessage();
            mm.To.Add(email);
            mm.Subject = subject;
            mm.Body = htmlMessage;

            mm.IsBodyHtml = true;
            mm.From = new MailAddress(Global.SMTP.User);

            SmtpClient smtp = new SmtpClient(Global.SMTP.Server);
            //smtp.Port = 587;
            smtp.UseDefaultCredentials = true;

            //smtp.EnableSsl = true;
            // These depend on the requirements of the server
            //smtp.EnableSsl = true;
            //smtp.Port = 465;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(Global.SMTP.User, Global.SMTP.Password);

            return smtp.SendMailAsync(mm);
        }
    }
}
