using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Email
{
    public interface IEmailService
    {
        bool sendMail(string to, string subject, string title, string text,string token);
        bool sendMail(string to, string subject, string title, string text,string token, string buttonText, string buttonUrl);
    }
}
