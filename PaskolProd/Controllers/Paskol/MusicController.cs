using Microsoft.AspNet.Identity.Owin;
using Model;
using Model.Authentication;
using Model.Confirm;
using Model.Controllers.Helper;
using Model.Filters;
using Newtonsoft.Json;
using PaskolProd.Authentication;
using PaskolProd.Controllers.Helper;
using PaskolProd.Models;
using Services;
using Services.Messaging.Requests;
using Services.Messaging.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PaskolProd.Paskol.Controllers
{
    public class MusicController : Controller
    {
        MusicService _mscService;
        TagService _tgService;
        ConfirmationService _cnfrmService;
        PaskolEmailService _mailService;
        

        const string MUSIC_URL_FORMAT = "{0}Search?id={1}";
        private readonly int RESULTS_IN_PAGE = 25;

        internal PaskolUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<PaskolUserManager>();
            }
        }

        public MusicController()
        {
            _mscService = ServiceLocator.GetService<MusicService>();
            _mailService = ServiceLocator.GetService<PaskolEmailService>();
            _tgService = ServiceLocator.GetService<TagService>();
            _cnfrmService = ServiceLocator.GetService<ConfirmationService>();
        }

        [HttpPost]
        [Authorize]
        [AJAXValidateAntiForgeryToken]
        public ActionResult MusicFavourite(int id)
        {
            var user = User.GetUser();
            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            ResponseBase res = _mscService.AddOrRemoveFavouriteMusic(id, user);
            return Json(new { Success = res.Success, Errors = res.Message });
        }

        [HttpPost]
        [Authorize]
        public ActionResult FavouriteMusics(int page = 1)
        {
            var user = User.GetUser();
            MusicSearchResult res = new MusicSearchResult();
            var favouriteMusics = user.FavouriteMusics.
                Where(m => m.NCDataUpload == null &&
                            (m.Status == MusicActiveStatus.Edited ||
                                    m.Status == MusicActiveStatus.Public));
            res.TotalResults = favouriteMusics.Count();
            res.TotalPages = (int)Math.Ceiling((double)res.TotalResults / RESULTS_IN_PAGE);
            
            res.Musics= favouriteMusics?.
                Skip((page - 1) * RESULTS_IN_PAGE).
                Take(RESULTS_IN_PAGE).
                Select(m=> new MusicSearchModel(m, true)).
                ToList();
            return Json(res);
        }

        [Authorize(Roles = "Artist")]
        [HttpPost]
        public ActionResult MySongs(bool sortByDateDesc = false, int page = 1)
        {
            var user = User.GetUser();
            MusicSearchResult res = new MusicSearchResult();
            var artistMusics = user.Artist.ArtistMusics
                .Where(m=> m.NCDataUpload == null &&
                           m.Status != MusicActiveStatus.Edited);
            if (sortByDateDesc)
            {
                artistMusics = artistMusics.OrderByDescending(m => m.CreateDate);
            }
            else
            {
                artistMusics = artistMusics.OrderBy(m => m.CreateDate);
            }
            res.TotalResults = artistMusics.Count();
            res.TotalPages = (int)Math.Ceiling((double)res.TotalResults / RESULTS_IN_PAGE);
            res.Musics = artistMusics
                .Select(m=> new MusicSearchModel(m,false,true))
                .Skip((page -1) * RESULTS_IN_PAGE)
                .Take(RESULTS_IN_PAGE);
            return Content(JsonConvert.SerializeObject(res), "application/json");
        }

        [HttpPost]
        public ActionResult GetByID(int id)
        {
            var res= _mscService.GetByID(id);
            if (res.Success)
            {
                var user = User.GetUser();
                bool isFavourite = false;
                if (user?.FavouriteMusics != null)
                {
                    isFavourite = user.FavouriteMusics.FirstOrDefault(msc => msc.ID == id) != null;
                }

                var resMusic = new MusicSearchModel(res.Entity, isFavourite);
                return Json(resMusic);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, res.Message);
            }
        }
        [HttpPost]
        [Authorize(Roles ="Artist")]
        [AJAXValidateAntiForgeryToken]
        public ActionResult SaveMusic(string editedMusic,HttpPostedFileBase WAVFile, HttpPostedFileBase MP3File, string tagIdsStr)
        {
            ResponseBase res = new ResponseBase();
            var serl = new System.Web.Script.Serialization.JavaScriptSerializer();
            MusicSearchModel music = serl.Deserialize<MusicSearchModel>(editedMusic);
            var dbResTags = new List<Tag>();
            if (tagIdsStr != null && tagIdsStr.Length >0)
            {
                IEnumerable<Guid> tagIds = tagIdsStr.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(t => Guid.Parse(t));
                if (tagIds.Any())
                {
                    dbResTags = _tgService.GetTagList(tagIds)
                    .Entities
                    .Where(t => t.IsPublicTag)
                    .ToList();
                }
            }
            
            var user = User.GetUser();
            MusicHelper mh = new MusicHelper();
            if (music.ID == null)
            {
                Music msc = new Music();
                msc.CreateDate = DateTime.Now;
                msc.ArtistID = user.Id;
                msc.Artist = user.Artist;
                msc.Status = MusicActiveStatus.New;
                msc.Tags = dbResTags;
                var newInSystemTagRes = _tgService.GetNewInSystemTag();
                if (newInSystemTagRes.Success)
                {
                    msc.Tags.Add(newInSystemTagRes.Entity);
                }
                var files = mh.SetMusicBeforeUpDateForArtist(msc, music, WAVFile, MP3File);
                res = _mscService.Add(msc, files.wavFile, files.mp3File);
                if (res.Success)
                {
                    var confirm = new Confirmation();
                    confirm.ConfirmType = ConfirmType.NewMusic;
                    confirm.DateUpdate = DateTime.Now;
                    confirm.Name = msc.HebrewName;
                    confirm.EntityId = msc.ID.ToString();
                    res = _cnfrmService.Add(confirm);
                }
            }
            else
            {
                var getMusicRes = _mscService.GetByID(music.ID.Value);
                if (getMusicRes.Success && getMusicRes.Entity.ArtistID == user.Id)
                {
                    var msc = new Music(getMusicRes.Entity);
                    msc.Tags =  msc.Tags
                        .Where(t => !t.IsPublicTag)
                        .Union(dbResTags)
                        .ToList();
                    var files = mh.SetMusicBeforeUpDateForArtist(msc, music, WAVFile, MP3File);
                    
                    if (msc.Status == MusicActiveStatus.Public)
                    {
                        msc.Status = MusicActiveStatus.WaitingForConfirm;
                        msc.SourceMusicId = getMusicRes.Entity.ID;
                        msc.ID = 0;
                        res = _mscService.Add(msc, files.wavFile, files.mp3File);
                        if (res.Success)
                        {
                            getMusicRes.Entity.Status = MusicActiveStatus.Edited;
                            var confirm = new Confirmation();
                            confirm.ConfirmType = ConfirmType.UpdateMusic;
                            confirm.DateUpdate = DateTime.Now;
                            confirm.Name = msc.HebrewName;
                            confirm.EntityId = msc.ID.ToString();
                            res = _cnfrmService.Add(confirm);
                        }
                    }
                    else if (msc.Status == MusicActiveStatus.New || msc.Status == MusicActiveStatus.WaitingForConfirm)
                    {
                        var confirmRes = _cnfrmService.GetByEntityId(msc.ID.ToString());
                        if (confirmRes.Success && confirmRes.Entity == null)
                        {
                            var confirm = new Confirmation();
                            confirm.ConfirmType =
                                msc.Status == MusicActiveStatus.New 
                                    ? ConfirmType.NewMusic 
                                    : ConfirmType.UpdateMusic;
                            confirm.DateUpdate = DateTime.Now;
                            confirm.Name = msc.HebrewName;
                            confirm.EntityId = msc.ID.ToString();
                            res = _cnfrmService.Add(confirm);
                        }
                        res = _mscService.Update(msc, msc.ID, files.wavFile, files.mp3File);
                    }
                }
                else
                {
                    res.Success = false;
                }
            }

            return Json(res);
        }

        [HttpPost]
        public ActionResult MusicByArtistId(string Id, int page = 1, SearchSortType sortType = SearchSortType.LengthDesc)
        {
            MusicSearchResult musicRes = new MusicSearchResult();

            PagingResponse<Music> res = _mscService.GetByArtistId(Id, sortType, new PagingRequest(page, RESULTS_IN_PAGE));

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

            return Json(musicRes);
        }

        [HttpPost]
        [PreventRepeatedRequet]
        [AJAXValidateAntiForgeryToken]
        public ActionResult ShareInMail(ShareInMailModel model)
        {
            bool success = true;
            if (ModelState.IsValid)
            {
                string appPath = CommonHelper.GetBaseUrl();

                var url = string.Format(MUSIC_URL_FORMAT, appPath, model.MusicId);
                success = _mailService.ShareMusic(model.UserMail, model.FriendMail, model.Comments, model.SendCopy, url);

                return Json(new { Success = success });
            }
            else
            {
                success = false;
                var errors = ModelState.SelectMany(x => x.Value.Errors);
                //res.Message = string.Join(Environment.NewLine, errors);
                return Json(new { Success = success, errors = errors });
            }
        }
    }
}