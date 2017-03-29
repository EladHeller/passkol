using Model;
using Model.Users;
using Services;
using Services.Messaging.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaskolProd.Paskol.Controllers
{
    public class ArtistsController : Controller
    {
        private UsersService _usrService;
        public ArtistsController()
        {
            _usrService = ServiceLocator.GetService<UsersService>();
        }
        public ActionResult Id(string id)
        {
            var res = _usrService.GetByID(id);
            ViewBag.Success = res.Success;
            ViewBag.Message = res.Message;
            ViewBag.PopularTags = GetArtistPopularTags(res.Entity?.Artist);
            return View("Index", res.Entity?.Artist);
        }

        public ActionResult UrlPage(string urlPage)
        {
            EntityResponse<PaskolUser> res = _usrService.GetArtistByUrlPage(urlPage);
            ViewBag.Success = res.Success;
            ViewBag.Message = res.Message;
            ViewBag.PopularTags = GetArtistPopularTags(res.Entity?.Artist);
            return View("Index", res.Entity?.Artist);
        }

        public List<string> GetArtistPopularTags(Artist artist)
        {
            List<string> popularTags = null;
            try
            {

            var activeMusics = artist?.ArtistMusics.Where(m =>
                (m.Status == MusicActiveStatus.Public || m.Status == MusicActiveStatus.Edited) &&
                m.NCDataUpload == null);
            popularTags = activeMusics?.
                SelectMany(m => m.Tags)?.
                GroupBy(t => t.ID).
                OrderByDescending(grp => grp.Count()).
                Take(5).
                Select(grp => grp.First().Name).
                ToList();
            }
            catch (Exception e)
            {
                //TODO: Handle exeption
                popularTags = new List<string>() { "אירעה שגיא בעת שליפת התגיות"};
            }

            return popularTags;
        }
    }
}