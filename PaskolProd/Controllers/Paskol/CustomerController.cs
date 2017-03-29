using Model.Logs;
using PaskolProd.Authentication;
using PaskolProd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaskolProd.Paskol.Controllers
{
    [Authorize(Roles ="Customer")]
    public class CustomerController : Controller
    {
        // GET: Artist
        public ActionResult OriginalMusic()
        {
            return View();
        }

        public ActionResult MyProfile()
        {
            var user = User.GetUser();
            return View(new ProfileShared()
            {
                CompanyName = user.Customer.CompanyName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName
            });
        }

        public ActionResult PurchaseReport()
        {
            return View();
        }
        public ActionResult FavuoriteMusics()
        {
            return View();
        }
        public ActionResult UserSearches(bool sortDesc = true)
        {
            var user = User.GetUser();
            ViewBag.UserId = user.Id;
            IOrderedEnumerable<SearchLog> searchLogs =null;
            if (sortDesc)
            {
                searchLogs= user.SearchLogs?.OrderByDescending(s => s.DateTime);
                ViewBag.SortText = "תאריך (מהחדש לישן)";
            }
            else
            {
                searchLogs = user.SearchLogs?.OrderBy(s => s.DateTime);
                ViewBag.SortText = "תאריך (מהישן לחדש)";
            }
            ViewBag.MusicsCount = searchLogs.Count();
            return View(searchLogs.Take(20));
        }
    }
}