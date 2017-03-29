using Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaskolProd.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //PaskolContext p = new PaskolContext();

            // p.Artists.Add(new Artist() { ContactManName = "m" });
            //p.SaveChanges();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}