using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaskolProd.Paskol.Controllers
{
    public class InfoController : Controller
    {
        private PaskolEmailService _mailSrv;
        public InfoController()
        {
            _mailSrv = ServiceLocator.GetService<PaskolEmailService>();
        }
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MailToTeam(string firstName, string lastName, string email, string phone, string content)
        {
            _mailSrv.MailToTeam(firstName, lastName, email, phone, content);
            return View("Contact", true);
        }

        public ActionResult artists()
        {
            return View();
        }

        public ActionResult OriginalMusic()
        {
            return View();
        }

        public ActionResult buying_content()
        {
            return View();
        }

        public ActionResult faq()
        {
            return View();
        }

        public ActionResult how_it_works()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}