using PaskolProd.Models;
using Services;
using Services.Messaging.Responses;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Model.Users;
using Model;
using Model.Logs;
using PagedList;
using Services.Messaging.Requests;
using Model.Purchase;
using Services.Helpers;
using Model.Filters;

namespace PaskolProd.Areas.Admin.Controllers
{
    [Authorize(Roles = "SystemAdmin,AdminFinance")]
    public class ReportsController : UserBaseController
    {
        private const int RES_IN_PAGE = 50;
        private ReportService service;
        private PurchaseService PurchaseService;
        private UserActionService userActionLogSrv;
        private MusicService musicService;
        private NCRegisterService NCRegisterSrv;
        private NCDataUploadService NCDataUpload;
        private NCPurchaseService NCPurchasSrv;

        public ReportsController()
        {
            this.NCPurchasSrv = ServiceLocator.GetService<NCPurchaseService>();
            this.NCDataUpload = ServiceLocator.GetService<NCDataUploadService>();
            this.NCRegisterSrv = ServiceLocator.GetService<NCRegisterService>();
            this.userActionLogSrv = ServiceLocator.GetService<UserActionService>();
            this.service = ServiceLocator.GetService<ReportService>();
            this.PurchaseService = ServiceLocator.GetService<PurchaseService>();
            musicService = ServiceLocator.GetService<MusicService>();
        }

        public ActionResult Musics(MusicReportViewModel model)
        {
            EntityResponse<int> res = new EntityResponse<int>();
            model.EndDate = model.EndDate.HasValue ? model.EndDate.Value.AddDays(1) : DateTime.Now.AddDays(1);
            model.StartDate = model.StartDate.HasValue ? model.StartDate : DateTime.Now.AddDays(-7);
            switch (model.Type)
            {
                case MusicReportType.All:
                   res = musicService.GetMusicCount(null, null);
                    break;
                case MusicReportType.Show:
                    res = service.GetLogsCountForEntityAction(EntityType.Music, LogType.MusicView, model.StartDate, model.EndDate);
                    break;
                case MusicReportType.Purchase:
                    res = service.GetLogsCountForEntityAction(EntityType.Music, LogType.Purchase, model.StartDate, model.EndDate);
                    break;
            }
            model.EndDate = model.EndDate.Value.AddDays(-1);
            if (!res.Success)
            {
                ModelState.AddModelError("", "נכשלה הבאת כמות היצירות!");
            }
            else
            {
                model.Count = res.Entity;
            }
            return View(model);
        }

        public ActionResult LeadMusics(LeadMusicViewModel model)
        {
            if (model.Page == 0)
            {
                model.Page = 1;
            }
            var action = 
                (model.ActionType == MusicReportType.Purchase)
                ? LogType.Purchase : LogType.MusicView;

            if (!model.StartDate.HasValue)
            {
                model.StartDate = DateTime.Now.AddDays(-7);
            }
            bool isNow = false;
            if (!model.EndDate.HasValue)
            {
                isNow = true;
                model.EndDate = DateTime.Now;
            }

            var res = service.GetMusicLogByAction(action, 
                model.StartDate.Value, 
                // Include the last day
                isNow ? model.EndDate.Value : model.EndDate.Value.AddDays(1),
                new PagingRequest(model.Page, RES_IN_PAGE));
            
            if (res.Success)
            {
                model.LeadMusicLogs = res.Entities.ToList();
                model.TotalPages = res.TotalPages;
                model.TotalResults = res.TotalResults;
            }
            else
            {
                ModelState.AddModelError("", "נכשלה הבאת היצירות מהשרת!");
            }

            return View(model);
        }
        [HttpGet]
        public async Task<ActionResult> UserReport(string Id)
        {
            UserReportViewModel view = new UserReportViewModel();
            var User = await UserManager.Users.SingleOrDefaultAsync(u => u.Id == Id);

            if (User != null)
            {
                var res = this.service.LogsForUser(Id);
                var PurchaseRes = this.PurchaseService.GetByCustomerId(Id);

                if (res.Success && PurchaseRes.Success)
                {
                    view.User = User;
                    view.UserMusicLogs = res.UserMusicLogs;
                    view.UserWatchArtistLogs = res.UserWatchArtistLogs;
                    view.UserWatchTagsLogs = res.UserWatchTagsLogs;
                    view.Purchases = PurchaseRes.Entities.Where(p => p.PurchaseStatus != 
                        PurchaseStatus.NotPaidBycustomer);
                }
                else
                {
                    ModelState.AddModelError("", "שגיאה במהלך שליפת הנתונים");
                }
            }
            else
            {
                ModelState.AddModelError("", "משתמש לא נמצא");
            }

            return View(view);
        }

        public ActionResult Users(DateTime? StartDate, DateTime? EndDate, UsersStatusEnumView? usersStatus)
        {
            StartDate = StartDate == null ? DateTime.Now.AddDays(-7) : StartDate;
            EndDate = EndDate == null ? DateTime.Now : EndDate;
            int ArtistCount = 0;

            switch (usersStatus)
            {
                case UsersStatusEnumView.New:
                    ArtistCount = this.UserManager.Users.Where(u =>
                    (u.UserType == UserType.Artist) && 
                    (u.RegisteredDate >= StartDate.Value &&
                     u.RegisteredDate <= EndDate.Value)).Count();
                    break;
                case UsersStatusEnumView.Active:
                    var ulgRES = userActionLogSrv.GetByDateAndType(StartDate.Value, 
                        EndDate.Value.AddDays(1), UserType.Artist);
                    ArtistCount = ulgRES.Entity;
                    break;
                default:
                    ArtistCount = this.UserManager.Users.Where(u =>
                    (u.UserType == UserType.Artist)).Count();
                    break;
            }

            ViewBag.ArtistCount = ArtistCount;
            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;
            
            return View(new UsersReportViewModel());
        }

        public ActionResult PurchaseSum(DateTime? StartDate, DateTime? EndDate)
        {
            StartDate = StartDate == null ? DateTime.Now.AddDays(-7) : StartDate;
            EndDate = EndDate == null ? DateTime.Now : EndDate;
            
            var res = PurchaseService.GetBetweenDates(StartDate.Value, EndDate.Value, true);

            if (res.Success)
            {
                ViewBag.SUM_Purchase = res.Entities.Count();
                ViewBag.SUM_Earn = res.Entities.Sum(p => p.ArtistEarn + p.PasskolEarn);
                ViewBag.AVG_Earn = res.Entities.Average(p => p.ArtistEarn + p.PasskolEarn) ?? 0;
                ViewBag.AVG_ArtistEarn = res.Entities.Average(p => p.ArtistEarn) ?? 0;
                ViewBag.SUM_PasskolEarn = res.Entities.Sum(p => p.PasskolEarn);
            }
        
            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;

            return View();
        }

        public ActionResult MusicPurchase(int? MusicId)
        {
            if (!MusicId.HasValue)
            {
                return new HttpStatusCodeResult(400, "בקשה ללא נתונים"); 
            }
            var res = PurchaseService.GetByMusicId(MusicId.Value);

            if (!res.Success)
            {
                return Json("אירעה שגיאה בעת הבאת הנתונים", JsonRequestBehavior.AllowGet);
            }

            return View(res.Entities);
        }

        public ActionResult Purchase(DateTime? StartDate, DateTime? EndDate, int PageIndex = 1)
        {
            StartDate = StartDate == null ? DateTime.Now.AddDays(-7) : StartDate;
            EndDate = EndDate == null ? DateTime.Now : EndDate;

            var res = PurchaseService.Get(new PagingRequest(PageIndex, RES_IN_PAGE)
                , StartDate.Value, EndDate.Value, null, true);

            if (res.Success)
            {
                ViewBag.StartDate = StartDate;
                ViewBag.EndDate = EndDate;
                ViewBag.Page = PageIndex;
                ViewBag.TotalPages = res.TotalPages;
                return View(res.Entities);
            }

            return Json(res.Message, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PurchaseArtistsNotPaid(DateTime? StartDate, DateTime? EndDate, int PageIndex = 1)
        {
            StartDate = StartDate == null ? DateTime.Now.AddDays(-7) : StartDate;
            EndDate = EndDate == null ? DateTime.Now : EndDate;

            var res = PurchaseService.Get(new PagingRequest(PageIndex, RES_IN_PAGE)
                , StartDate.Value, EndDate.Value, PurchaseStatus.NotPaidToArtist,false);

            if (res.Success)
            {
                ViewBag.StartDate = StartDate;
                ViewBag.EndDate = EndDate;

                ViewBag.Page = PageIndex;
                ViewBag.TotalPages = res.TotalPages;
                return View(res.Entities);
            }

            return Json(res.Message, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult PurchaseArtistsNotPaid(Purchase pToUpdate)
        {
            if (ModelState.IsValid) 
            {
                var res = PurchaseService.UpdatePaidToArtist(pToUpdate);
            }

            return RedirectToAction("PurchaseArtistsNotPaid");
        }

        [HttpGet]
        public ActionResult RegisterNotCompleted(DateTime? StartDate, DateTime? EndDate, int PageIndex = 1)
        {
            StartDate = StartDate == null ? DateTime.Now.AddDays(-7) : StartDate;
            EndDate = EndDate == null ? DateTime.Now : EndDate;

            var res = this.NCRegisterSrv.Get(new PagingRequest(PageIndex, RES_IN_PAGE)
                , StartDate.Value, EndDate.Value, NCUserType.Register);

            if (res.Success)
            {
                ViewBag.StartDate = StartDate;
                ViewBag.EndDate = EndDate;

                ViewBag.Page = PageIndex;
                ViewBag.TotalPages = res.TotalPages;
                return View(res.Entities);
            }

            return Json(res.Message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RegisterNotCompleted(string StartDate, string EndDate, string note, string Id, NCAction action = NCAction.save)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                var res = this.NCRegisterSrv.SaveByStatus(note, action, Id);
            }

            return RedirectToAction("RegisterNotCompleted",new { StartDate = StartDate, EndDate = EndDate});
        }
        
        [HttpGet]
        public ActionResult ArtistNotCompleted(DateTime? StartDate, DateTime? EndDate, int PageIndex = 1)
        {
            StartDate = StartDate == null ? DateTime.Now.AddDays(-7) : StartDate;
            EndDate = EndDate == null ? DateTime.Now : EndDate;

            var res = this.NCRegisterSrv.Get(new PagingRequest(PageIndex, RES_IN_PAGE)
                , StartDate.Value, EndDate.Value, NCUserType.ArtistProfile);

            if (res.Success)
            {
                ViewBag.StartDate = StartDate;
                ViewBag.EndDate = EndDate;

                ViewBag.Page = PageIndex;
                ViewBag.TotalPages = res.TotalPages;
                return View(res.Entities);
            }

            return Json(res.Message, JsonRequestBehavior.AllowGet);
            }

        [HttpPost]
        public ActionResult ArtistNotCompleted(string StartDate, string EndDate, string note, string Id, NCAction action = NCAction.save)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                var res = this.NCRegisterSrv.SaveByStatus(note, action, Id);
            }

            return RedirectToAction("ArtistNotCompleted", new { StartDate = StartDate, EndDate = EndDate });
        }

        [HttpGet]
        public ActionResult DataUploadNotCompleted(DateTime? StartDate, DateTime? EndDate, int PageIndex = 1)
        {
            StartDate = StartDate == null ? DateTime.Now.AddDays(-7) : StartDate;
            EndDate = EndDate == null ? DateTime.Now : EndDate;

            var res = this.NCDataUpload.Get(new PagingRequest(PageIndex, RES_IN_PAGE)
                , StartDate.Value, EndDate.Value);

            if (res.Success)
            {
                ViewBag.StartDate = StartDate;
                ViewBag.EndDate = EndDate;

                ViewBag.Page = PageIndex;
                ViewBag.TotalPages = res.TotalPages;
            return View(res.Entities);
        }

            return Json(res.Message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DataUploadNotCompleted(string StartDate, string EndDate, string note, int? Id, NCAction action = NCAction.save)
        {
            if (Id != null)
            {
                var res = this.NCDataUpload.SaveByStatus(note, action, Id.Value);
            }

            return RedirectToAction("DataUploadNotCompleted", new { StartDate = StartDate, EndDate = EndDate });
        }

        [HttpGet]
        public ActionResult PurchasesNotCompleted(DateTime? StartDate, DateTime? EndDate, int PageIndex = 1)
        {
            StartDate = StartDate == null ? DateTime.Now.AddDays(-7) : StartDate;
            EndDate = EndDate == null ? DateTime.Now : EndDate;

            var res = this.NCPurchasSrv.Get(new PagingRequest(PageIndex, RES_IN_PAGE)
                , StartDate.Value, EndDate.Value);

            if (res.Success)
            {
                ViewBag.StartDate = StartDate;
                ViewBag.EndDate = EndDate;

                ViewBag.Page = PageIndex;
                ViewBag.TotalPages = res.TotalPages;
                return View(res.Entities);
            }
            
            return Json(res.Message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PurchasesNotCompleted(string StartDate, string EndDate, string note, string Id, NCAction action = NCAction.save)
        {
            if (Id != null)
            {
                var res = this.NCPurchasSrv.SaveByStatus(note, action, Id);
            }

            return RedirectToAction("PurchasesNotCompleted", new { StartDate = StartDate, EndDate = EndDate });
        }
    }
}