using PaskolProd.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaskolProd.Paskol.Controllers
{
    [Authorize(Roles = "Artist")]
    public class ArtistController : Controller
    {
        // GET: Artist
        public ActionResult MyProfile()
        {
            return View(User.GetUser().Artist);
        }

        public ActionResult MySongs()
        {
            Response.Cache.SetCacheability(HttpCacheability.Private);
            Response.Cache.SetMaxAge(new TimeSpan(0));
            return View(User.GetUser());
        }

        public ActionResult SellReport()
        {
            return View();
        }

        public ActionResult OriginalMusic()
        {
            return View(User.GetUser().Artist.ParticipateInAuction);
        }

        public ActionResult PersonalManagment()
        {
            return View(User.GetUser().Artist);
        }
    }
}