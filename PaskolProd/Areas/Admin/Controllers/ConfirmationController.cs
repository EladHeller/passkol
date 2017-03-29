using PaskolProd.Models;
using PaskolProd.Models.Helper;
using Services;
using Services.Messaging.Requests;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using Model.Confirm;
using Model.Users;
using Model.Purchase;
using Model;
using System.Net;
using System.Linq;
using System.Web;
using PaskolProd.Controllers.Helper;
using System;
using Model.Filters;
using Model.Helper;
using Model.Authentication;
using System.Collections.Generic;

namespace PaskolProd.Areas.Admin.Controllers
{  

    public class ConfirmationController : ConfirmBaseController
    {
        const int RES_IN_PAGE = 25;
        private FileSystemService FSservice;
        public ConfirmationController()
        {
            FSservice = ServiceLocator.GetService<FileSystemService>();
        }

        [Authorize(Roles = "AdminLabel,SystemAdmin,AdminMusicEditor,AdminProfile,AdminFinance")]
        // GET: Confirmation
        public ActionResult Index(ConfirmSorting sortOrder = ConfirmSorting.date_desc,
            ConfirmType ConfirmType = ConfirmType.All,
            int PageIndex = 1)
        {
            // Set sorting params
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TypeSortParm = sortOrder == ConfirmSorting.type ? ConfirmSorting.type_desc : ConfirmSorting.type;
            ViewBag.DateSortParm = sortOrder == ConfirmSorting.date ? ConfirmSorting.date_desc : ConfirmSorting.date;
            ViewBag.SelectedType = ConfirmType;
            List<ConfirmType> ConfirmTypes = new List<ConfirmType>();

            if (ConfirmType == ConfirmType.All)
            {
                if (User.IsInRole("SystemAdmin"))
                {
                    ConfirmTypes = null;
                }
                if (User.IsInRole("AdminProfile"))
                {
                    ConfirmTypes.Add(ConfirmType.NewArtist);
                    ConfirmTypes.Add(ConfirmType.UpdateArtist);
                }
                if (User.IsInRole("AdminFinance"))
                {
                    ConfirmTypes.Add(ConfirmType.PhonePurchase);
                }
                if (User.IsInRole("AdminMusicEditor") || User.IsInRole("AdminLabel"))
                {
                    ConfirmTypes.Add(ConfirmType.NewMusic);
                    ConfirmTypes.Add(ConfirmType.UpdateMusic);
                }
            }
            else
            {
                ConfirmTypes.Add(ConfirmType);
            }

            var res = service.GetBySearch(ConfirmTypes, new PagingRequest(PageIndex, RES_IN_PAGE), sortOrder);

            if (!res.Success)
            {
                return Json(res.Message, JsonRequestBehavior.AllowGet);
            }

            ViewBag.Page = PageIndex;
            ViewBag.TotalPages = res.TotalPages;
            return View(new ConfirmationViewModel() { Confirmations = res.Entities, ConfirmType = ConfirmType });
        }

        [HttpPost]

        [Authorize(Roles = "SystemAdmin,AdminProfile")]
        public async Task<ActionResult> EditedArtist(PaskolUser ReqUser, ConfirmTypeAction confirmAction,
            string DeclineReason)
        {
            if (ModelState.IsValid)
            {
                PaskolUser user = UserManager.Users.SingleOrDefault(u => u.Id == ReqUser.Id);

                user.Artist.TempArtist.UserName = ReqUser.Artist.TempArtist.UserName;
                user.Artist.TempArtist.Email = ReqUser.Artist.TempArtist.Email;

                if (user != null)
                {
                    if (await ConfirmEditedArtist(user, confirmAction, DeclineReason))
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User Not Found");
                }
            }

            return View(ReqUser);
        }

        [HttpGet]

        [Authorize(Roles = "SystemAdmin,AdminProfile")]
        public async Task<ActionResult> EditedArtist(string Id)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                PaskolUser user = await UserManager.Users.Include("Artist.TempArtist")
                    .SingleOrDefaultAsync(u => u.Id == Id);

                if (user != null)
                {
                    // Save url to geet back to after action ends
                    ViewBag.ReturnURL = Request.UrlReferrer;
                    return View(user);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]

        [Authorize(Roles = "SystemAdmin,AdminProfile")]
        public async Task<ActionResult> NewArtist(string Id)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                PaskolUser user = await UserManager.FindByIdAsync(Id);

                if (user != null)
                {
                    // Save url to geet back to after action ends
                    ViewBag.ReturnURL = Request.UrlReferrer;
                    return View(new ArtistEditView(user));
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]

        [Authorize(Roles = "SystemAdmin,AdminProfile")]
        public async Task<ActionResult> NewArtist(ArtistEditView EditedArtist, ConfirmTypeAction confirmAction,
            string DeclineReason, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                PaskolUser user = await UserManager.FindByIdAsync(EditedArtist.UserID);

                if (user != null)
                {
                    // Update user detials
                    EditedArtist.UserFromArtistEditView(user);
                    // Save photo if have
                    var PhotoRes = FSservice.StorePhoto(upload, user);

                    if (PhotoRes.Success)
                    {
                        user.Artist.PhotoID = PhotoRes.PhotoId;

                        // If succeded
                        if (await ConfirmNewArtist(user, confirmAction, EditedArtist.Password, DeclineReason))
                        {
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("PictureId", PhotoRes.Message);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User Not Found");
                }
            }
            return View();
        }

        [HttpGet]

        [Authorize(Roles = "SystemAdmin,AdminFinance")]
        public ActionResult PhonePurchase(string Id)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                if (TempData["ViewData"] != null)
                {
                    ViewData = (ViewDataDictionary)TempData["ViewData"];
                }

                var PurchaseRes = purchaseService.GetByID(Id);

                if (PurchaseRes.Success)
                {
                    // Save url to geet back to after action ends
                    ViewBag.ReturnURL = Request.UrlReferrer;
                    return View(PurchaseRes.Entity);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]

        [Authorize(Roles = "SystemAdmin,AdminFinance")]
        public ActionResult PhonePurchase(Purchase Purchase, ConfirmTypeAction confirmAction)
        {
            if (ModelState.IsValid)
            {
                var res = service.ConfirmPhonePurchase(Purchase, confirmAction);

                if (res.Success)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", res.Message);
                // Save model state in order to represent it under Redirection
                TempData["ViewData"] = ViewData;

                return RedirectToAction("PhonePurchase", new { Id = Purchase.PurchaseId });
            }

            return View();
        }

        [HttpGet]
        [Authorize(Roles = "SystemAdmin,AdminMusicEditor")]
        public ActionResult NewMusic(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var res = musicSrv.GetByID(id.Value);

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
            // Save url to geet back to after action ends
            ViewBag.ReturnURL = Request.UrlReferrer;
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "SystemAdmin,AdminMusicEditor")]
        public ActionResult NewMusic(MusicViewModelEntry music, HttpPostedFileBase WAVUpload, 
            HttpPostedFileBase MP3Upload, ConfirmTypeAction confirmAction, string DeclineReason)
        {
            if (ModelState.IsValid)
            {
                MusicHelper mh = new MusicHelper();
                var FilesFromHttp = mh.SetMusicBeforeUpDate(music, WAVUpload, MP3Upload);

                var res = service.ConfirmNewMusic(music, FilesFromHttp.wavFile, FilesFromHttp.mp3File,
                    confirmAction, DeclineReason);

                if (res.Success)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", res.Message);
            }

            return View(music);
        }

        [HttpGet]

        [Authorize(Roles = "SystemAdmin,AdminMusicEditor,")]
        public ActionResult EditedMusic(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var res = musicSrv.GetByID(id.Value);

            if (!res.Success)
            {
                return Json(res.Message, JsonRequestBehavior.AllowGet);
            }

            var Edited = res.Entity;

            if (Edited == null)
            {
                return HttpNotFound();
            }

            ConfEditMusicViewModel viewModel = new ConfEditMusicViewModel()
            {
                EditedMusic = new MusicViewModelEntry(Edited)
            };
            // Save url to geet back to after action ends
            ViewBag.ReturnURL = Request.UrlReferrer;
            return View(viewModel.EditedMusic);
        }

        [HttpPost]

        [Authorize(Roles = "SystemAdmin,AdminMusicEditor")]
        public ActionResult EditedMusic(MusicViewModelEntry music, HttpPostedFileBase WAVUpload,
            HttpPostedFileBase MP3Upload, ConfirmTypeAction confirmAction, string DeclineReason)
        {
            if (ModelState.IsValid)
            {
                MusicHelper mh = new MusicHelper();

                var FilesFromHttp = mh.SetMusicBeforeUpDate(music, WAVUpload, MP3Upload);

                var res = service.ConfirmEditedMusic(music, FilesFromHttp.wavFile, FilesFromHttp.mp3File, 
                    confirmAction, DeclineReason);

                if (res.Success)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", res.Message);
            }

            return View(music);
        }
    }
}