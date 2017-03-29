using Microsoft.AspNet.Identity;
using Model.Filters;
using PaskolProd.Areas.Admin.Controllers;
using PaskolProd.Authentication;
using PaskolProd.Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PaskolProd.Paskol.Controllers
{
    public class ArtistAgentController : Controller
    {
        private UsersService _userSrv = ServiceLocator.GetService<UsersService>();

        [Authorize(Roles = "ArtistAgent,SystemAdmin")]
        public ActionResult MyProfile()
        {
            var user = User.GetUser();
            return View(new ProfileShared()
            {
                CompanyName = user.ArtistAgent.CompanyName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName
            });
        }

        [Authorize(Roles = "ArtistAgent,SystemAdmin")]
        public ActionResult PersonalManagment()
        {
            return View(User.GetUser().ArtistAgent.Artists.Select(a => new ArtistAgentArtistsView()
            {
                ArtistId = a.User.Id,
                ArtistEmail = a.User.Email,
                ArtistsName = a.User.UserName,
                ArtistsAgentConfirmed = a.RelConfByArtistAgent,
                ArtistsConfirmed = a.RelConfByArtist
            }).ToList());
        }

        [Authorize(Roles = "ArtistAgent,SystemAdmin")]
        public ActionResult SellReport()
        {
            return View();
        }

        [Authorize(Roles = "Artist,SystemAdmin")]
        public ActionResult AgntConfirmedByArt(string ArtistAgentId)
        {
            HttpStatusCodeResult HttpRes = 
                new HttpStatusCodeResult(HttpStatusCode.InternalServerError);

            if (!string.IsNullOrEmpty(ArtistAgentId))
            {
                var res = _userSrv.AgencyConfirmedBy(true, ArtistAgentId, User.Identity.GetUserId());

                if (res.Success)
                {
                    HttpRes = new HttpStatusCodeResult
                        (HttpStatusCode.OK, "הסוכן אושר בהצלחה!");
                }
                else
                {
                    HttpRes = new HttpStatusCodeResult
                       (HttpStatusCode.InternalServerError, res.Message);
                }

            }

            return HttpRes;
        }

        [Authorize(Roles = "ArtistAgent,SystemAdmin")]
        public ActionResult AgntConfirmedByArtAgnt(string ArtistId)
        {
            HttpStatusCodeResult HttpRes = 
                new HttpStatusCodeResult(HttpStatusCode.InternalServerError);

            if (!string.IsNullOrEmpty(ArtistId))
            {
                var res = _userSrv.AgencyConfirmedBy(false, User.Identity.GetUserId(), ArtistId);

                if (res.Success)
                {
                    HttpRes = new HttpStatusCodeResult
                        (HttpStatusCode.OK, "הסוכן אושר בהצלחה!");
                }
                else
                {
                    HttpRes = new HttpStatusCodeResult
                       (HttpStatusCode.InternalServerError, res.Message);
                }

            }

            return HttpRes;
        }

        [Authorize(Roles = "ArtistAgent,SystemAdmin")]
        public ActionResult AgntConfirmedViaEmailByArtAgnt(string ArtistEmail)
        {
            if (!string.IsNullOrEmpty(ArtistEmail))
            {
                return Json(_userSrv.AgencyConfirmedViaMailBy(false, User.Identity.GetUserId(), ArtistEmail), JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        [Authorize(Roles = "Artist,SystemAdmin")]
        public ActionResult AgntConfirmedViaEmailByArt(string ArtistAgentEmail)
        {
            if (!string.IsNullOrEmpty(ArtistAgentEmail))
            {
                return Json(_userSrv.AgencyConfirmedViaMailBy(true, User.Identity.GetUserId(), ArtistAgentEmail), 
                    JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        [Authorize(Roles = "Artist,SystemAdmin")]
        public ActionResult AgntRemovedByArt(string ArtistAgentId)
        {
            HttpStatusCodeResult HttpRes = 
                new HttpStatusCodeResult(HttpStatusCode.InternalServerError);

            if (!string.IsNullOrEmpty(ArtistAgentId))
            {
                var res = _userSrv.AgencyRemovedBy(true, ArtistAgentId, User.Identity.GetUserId());

                if (res.Success)
                {
                    HttpRes = new HttpStatusCodeResult
                        (HttpStatusCode.OK, "האמן הוסר בהצלחה!");
                }
                else
                {
                    HttpRes = new HttpStatusCodeResult
                       (HttpStatusCode.InternalServerError, res.Message);
                }
            }

            return HttpRes;
        }

        [Authorize(Roles = "ArtistAgent,SystemAdmin")]
        public ActionResult AgntRemovedByArtAgnt(string ArtistId)
        {
            HttpStatusCodeResult HttpRes =
                new HttpStatusCodeResult(HttpStatusCode.InternalServerError);

            if (!string.IsNullOrEmpty(ArtistId))
            {
                var res = _userSrv.AgencyRemovedBy(false, User.Identity.GetUserId(), ArtistId);
             
                if (res.Success)
                {
                    HttpRes = new HttpStatusCodeResult
                        (HttpStatusCode.OK, "האמן הוסר בהצלחה!");
                }
                else
                {
                    HttpRes = new HttpStatusCodeResult
                       (HttpStatusCode.InternalServerError, res.Message);
                }
            }

            return HttpRes;
        }
    }
}