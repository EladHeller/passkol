using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Model.Controllers.Paskol
{
    public class EmailController : Controller
    {
        PaskolEmailService srv = ServiceLocator.GetService<PaskolEmailService>();
        // GET: Email
        public ActionResult Unsubscribe(string Email, string Token)
        {
            ViewBag.Success = srv.UnsubscribeMail(Email, Token);
            return View();
        }
    }
}
