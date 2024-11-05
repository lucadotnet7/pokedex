using System;
using System.Net;
using System.Net.Mail;

namespace Services
{
    public sealed class EmailService
    {
        private MailMessage email;
        private SmtpClient smtpClient;

        public EmailService()
        {
            smtpClient = new SmtpClient();
            smtpClient.Credentials = new NetworkCredential("ff01e6ec42e120", "78e6c7a7b640fb");
            smtpClient.EnableSsl = true;
            smtpClient.Port = 2525;
            smtpClient.Host = "smtp.mailtrap.io";
        }

        public void CreateEmail(string destiny, string affair, string body)
        {
            email = new MailMessage();
            email.From = new MailAddress("lucadanielcanas7@gmail.com");
            email.To.Add(destiny);
            email.Subject = affair;
            email.IsBodyHtml = true;
            email.Body = body;
        }

        public void SendEmail()
        {
            try
            {
                smtpClient.Send(email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
