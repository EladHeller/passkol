using Model;
using Model.Filters;
using PaskolProd.Controllers.Helper;
using PaskolProd.Models;
using Services;
using Services.Messaging.Requests;
using Services.Messaging.Responses;
using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PaskolProd.Areas.Admin.Controllers
{
    [Authorize(Roles = "SystemAdmin,AdminMusicEditor")]
    public class MusicController : Controller
    {
        private MusicService service;
        public MusicController()
        {
            service = ServiceLocator.GetService<MusicService>();
        }
        
        // GET: Musics
        public ActionResult Index(string searchMusicName, 
            string searchArtistName,
            bool? success,
            int page = 1,
            MusicFieldsSort sortField = MusicFieldsSort.UpdateDate, 
            bool sortDesc = true)
        {
            var res = service.GetBySearch(searchMusicName,
                searchArtistName,
                new PagingRequest(page,25),
                sortField, 
                sortDesc);
            if (!res.Success)
            {
                return Json(res.Message, JsonRequestBehavior.AllowGet);
            }
            MusicViewModel viewModel = new MusicViewModel();
            viewModel.Success = success;
            viewModel.Musics = res.Entities.Select(msc => new MusicViewModelEntry(msc));
            ViewBag.SearchArtistName = searchArtistName;
            ViewBag.SearchMusicName = searchMusicName;
            ViewBag.Page = page;
            ViewBag.TotalPages = res.TotalPages;
            ViewBag.SortField = sortField;
            ViewBag.SortDesc = sortDesc;
            return View(viewModel);
        }
        public JsonResult SearchMusicsNames(string startWith)
        {
            var res = this.service.GetMusicNames(startWith,10);

            if (!res.Success)
            {
                return Json(res.Message, JsonRequestBehavior.AllowGet);
            }

            return Json(res.Entities, JsonRequestBehavior.AllowGet);
        }
        //// GET: Musics/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var res = service.GetByID(id.Value);
        //    if (!res.Success)
        //    {
        //        return Json(res.Message, JsonRequestBehavior.AllowGet);
        //    }
        //    Music music = res.Entity;
        //    if (music == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(music);
        //}

        // GET: Musics/Create
        public ActionResult Create()
        {
            MusicViewModelEntry viewModel = new MusicViewModelEntry()
            {
                Status = MusicActiveStatus.New,
                CreateDate = DateTime.Now,
            };

            return View(viewModel);
        }

        // POST: Musics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MusicViewModelEntry music, HttpPostedFileBase WAVUpload, HttpPostedFileBase MP3Upload, string ArtistName)
        {
            if (ModelState.IsValid)
            {
                MusicHelper mh = new MusicHelper();
                var FilesFromHttp = mh.SetMusicBeforeUpDate(music, WAVUpload, MP3Upload);
                var addMusic = new Music(music);

                var res = service.Add(addMusic, FilesFromHttp.wavFile, FilesFromHttp.mp3File);
                if (!res.Success)
                {
                    ModelState.AddModelError("", res.Message);
                }
                else
                {
                    return RedirectToAction("Index", new { success = true });
                }
            }

            return View(music);
        }

        // GET: Musics/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var res = service.GetByID(id.Value);
            if (!res.Success)
            {
                return Json(res.Message, JsonRequestBehavior.AllowGet);
            }
            Music music = res.Entity;
            if (music == null)
            {
                return HttpNotFound();
            }
            MusicViewModelEntry viewModel = new MusicViewModelEntry(music);
            //ViewBag.ArtistID = new SelectList(db.Artists, "Id", "ContactManName", music.ArtistID);
            return View(viewModel);
        }

        // POST: Musics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MusicViewModelEntry music, HttpPostedFileBase WAVUpload, HttpPostedFileBase MP3Upload)
        {
            if (ModelState.IsValid)
            {
                var res = new ResponseBase();
                try
                {
                    MusicHelper mh = new MusicHelper();
                    var FilesFromHttp = mh.SetMusicBeforeUpDate(music, WAVUpload, MP3Upload);
                    res = service.Update(music, music.ID, FilesFromHttp.wavFile, FilesFromHttp.mp3File);
                }
                catch (Exception e)
                {
                    res.Success = false;
                    res.Message = e.ToString();
                }
                if (!res.Success)
                {
                    ModelState.AddModelError("", res.Message);
                }
                else
                {
                    return RedirectToAction("Index", new { success = true});
                }
            }

            return View(music);
        }

        

        public ActionResult Block(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var res = service.GetByID(id.Value);
            if (!res.Success)
            {
                return Json(res.Message, JsonRequestBehavior.AllowGet);
            }
            Music music = res.Entity;
            if (music == null)
            {
                return HttpNotFound();
            }

            return View(music);
        }

        // POST: Musics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Block(int id)
        {
            var res = service.GetByID(id);
            if (!res.Success)
            {
                ModelState.AddModelError("", res.Message);
                return View();
            }
            else
            {
                res.Entity.Status = res.Entity.Status == MusicActiveStatus.Blocked ? MusicActiveStatus.Public : MusicActiveStatus.Blocked;
                var updateRes = service.Update(res.Entity);

                if (!updateRes.Success)
                {
                    ModelState.AddModelError("", updateRes.Message);
                    return View();
                }
            }
            return RedirectToAction("Index",new { success = true });
        }
    }
}
