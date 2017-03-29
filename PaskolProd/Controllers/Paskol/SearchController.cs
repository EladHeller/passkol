using Model;
using PaskolProd.Authentication;
using PaskolProd.Models;
using Services;
using Services.Messaging.Requests;
using Services.Messaging.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaskolProd.Paskol.Controllers
{
    public class SearchController : Controller
    {
        private TagService _tgService;
        private MusicService _mscService;
        public ReportService _rptService { get; set; }
        const int RESULTS_IN_PAGE = 10;
        public SearchController()
        {
            _tgService = ServiceLocator.GetService<TagService>();
            _mscService = ServiceLocator.GetService<MusicService>();
            _rptService = ServiceLocator.GetService<ReportService>();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Tags()
        {
            var res = _tgService.GetAll();
            if (!res.Success)
            {
                return new HttpStatusCodeResult(500, "אירעה שגיאה בהבאת התגיות");
            }
            else
            {
                var tags = res.Entities.Select(tg => new TagViewModel(tg, tg.ID, false, false, false, false));
                return Json(tags);
            }
        }
        
        [HttpPost]
        public ActionResult Search(string searchText, string strTagIds, int page=1, SearchSortType sortType = SearchSortType.Relevant)
        {
            MusicSearchResult musicRes = new MusicSearchResult();

            if (!string.IsNullOrWhiteSpace(searchText) || !string.IsNullOrWhiteSpace(strTagIds))
            {
                var tagIds = strTagIds
                    .Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(id => Guid.Parse(id));
                PagingResponse<Music> res = 
                    _mscService.GetBySearch(searchText, tagIds, new PagingRequest(page, RESULTS_IN_PAGE), sortType);

                if (res.Success)
                {
                    var user = User.GetUser();
                    ICollection<Music> favouriteMusic = new List<Music>();
                    if (user?.FavouriteMusics != null)
                    {
                        favouriteMusic = user.FavouriteMusics;
                    }
                    musicRes.TotalPages = res.TotalPages;
                    musicRes.TotalResults = res.TotalResults;
                    musicRes.Musics = res.Entities.Select(msc => new MusicSearchModel(msc, favouriteMusic.Any(fav => fav.ID == msc.ID)));
                }
                else
                {
                    return new HttpStatusCodeResult(500, res.Message);
                }
            }

            return Json(musicRes);
        }
    }
}