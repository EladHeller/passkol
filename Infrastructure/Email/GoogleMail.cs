using Infrastructure.Configuration;
using System;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace Infrastructure.Email
{
    public class GoogleMail : IEmailService
    {
        public string[] mailParts;
        public string infoMailUser;
        public string infoMailPass;

        public GoogleMail()
        {
            mailParts = File.ReadAllText(HttpContext.Current.Server.MapPath("~/App_Data/resource/mail.html"), Encoding.UTF8).Split('^');
        }

        public bool sendMail(string to, string subject, string title, string text,string token)
        {
            return sendMail(to, subject, title, text, token, "", "");

        }

        public bool sendMail(string to, string subject, string title, string text, string token, string buttonText, string buttonUrl)
        {
            try
            {
                string mailContent = getMailTemplate(to, title, text,token, buttonText, buttonUrl);
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.IsBodyHtml = true;
                mail.From = new MailAddress(WebConf.EmailFromSystem);
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = mailContent;
                mail.IsBodyHtml = true;
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        
        private string getMailTemplate(string to, string title, string text,string token, string buttonText, string buttonUrl)
        {
            string mailContent = mailParts[0] + title + mailParts[1] + text + mailParts[2];
            if (!string.IsNullOrEmpty(buttonText))
            {
                mailContent += mailParts[3].Replace("[BUTTON_TEXT]", buttonText).Replace("[BUTTON_URL]", buttonUrl);
            }
            mailContent += mailParts[4];
            mailContent += Infrastructure.Helper.CommonHelper.GetBaseUrl() + "/Email/Unsubscribe?Email=" + to + "&Token=" +token;
            mailContent += mailParts[5];

            return mailContent;
        }
    }
}
