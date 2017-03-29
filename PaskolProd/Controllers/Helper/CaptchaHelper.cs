using BotDetect.Web.UI.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaskolProd.Controllers.Helper
{
    public static class CaptchaHelper
    {
        public static MvcCaptcha GetCaptcha(string captchaName)
        {
            MvcCaptcha captcha = new MvcCaptcha(captchaName);
            captcha.ImageStyle = BotDetect.ImageStyle.Graffiti;
            captcha.ImageSize = new System.Drawing.Size(75, 30);
            captcha.CodeLength = 5;
            
            return captcha;
        }
    }
}