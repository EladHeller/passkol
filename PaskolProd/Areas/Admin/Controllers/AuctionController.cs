using Model.Auctions;
using Model.Filters;
using PaskolProd.Models.Response;
using Services;
using Services.Messaging.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PaskolProd.Areas.Admin.Controllers
{
    public class AuctionController : UserBaseController
    {
        const int RES_IN_PAGE = 25;
        private AuctionService _actionSrv;
        private PermissionCategoryService _PCService;

        public AuctionController()
        {
            _actionSrv = ServiceLocator.GetService<AuctionService>();
            _PCService = ServiceLocator.GetService<PermissionCategoryService>();
        }

        // GET: Auction
        [Authorize(Roles = "SystemAdmin")]
        public ActionResult Index(AuctionSort sortOrder = AuctionSort.OpenDateDesc, int PageIndex = 1)
        {
            // Set sorting params
            ViewBag.CurrentSort = sortOrder;
            ViewBag.OpenDateSP = sortOrder == AuctionSort.OpenDate ? AuctionSort.OpenDateDesc : AuctionSort.OpenDate;
            ViewBag.CloseSP = sortOrder == AuctionSort.CloseDate ? AuctionSort.CloseDateDesc : AuctionSort.CloseDate;
            ViewBag.PickWinnerDateSP = sortOrder == AuctionSort.PickWinnerDate ? AuctionSort.PickWinnerDateDesc: AuctionSort.PickWinnerDate;
            ViewBag.StatusSP = sortOrder == AuctionSort.Status ? AuctionSort.StatusDesc : AuctionSort.Status;

            var res = _actionSrv.Get(new PagingRequest(PageIndex, RES_IN_PAGE), sortOrder);

            if (!res.Success)
            {
                return Json(res.Message, JsonRequestBehavior.AllowGet);
            }

            ViewBag.Page = PageIndex;
            ViewBag.TotalPages = res.TotalPages;
            return View(res.Entities);
        }

        [HttpGet]
        [Authorize(Roles = "SystemAdmin")]
        public ActionResult Watch(string Id)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                var res = _actionSrv.GetByID(Id);

                if (res.Success && res.Entity != null)
                {
                    // Save url to geet back to after action ends
                    ViewBag.ReturnURL = Request.UrlReferrer;
                    return View(res.Entity);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [PreventRepeatedRequet]
        [Authorize(Roles = "SystemAdmin")]
        public ActionResult Close(string Id, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    var res = _actionSrv.GetByID(Id);

                    if (res.Success && res.Entity != null)
                    {
                        _actionSrv.closeAuction(res.Entity);
                        return Redirect(returnUrl);
                    }
                }

                return RedirectToAction("Index");
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        [Authorize(Roles = "SystemAdmin")]
        public ActionResult Confirm(string Id)  
        {
            if (!string.IsNullOrEmpty(Id))
            {
                var res = _actionSrv.GetByID(Id);

                if (res.Success && res.Entity != null)
                {
                    // Check if we have some store modelstate
                    if (TempData["ViewData"] != null)
                    {
                        ViewData = (ViewDataDictionary)TempData["ViewData"];
                    }
                    
                    // Save url to geet back to after action ends
                    ViewBag.ReturnURL = Request.UrlReferrer;
                    return View(res.Entity);
                }
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Confirm only artist 
        /// </summary>
        /// <param name="au"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "SystemAdmin")]
        public ActionResult Confirm(Auction au, string UrlReferrer)
        {
            if (au != null && !string.IsNullOrEmpty(au.AuctionId))
            {
                if (au.Participants != null)
                {
                    var ConfirmRes = this._actionSrv.ConfirmAuction(au);

                    if (ConfirmRes.Success)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", ConfirmRes.Message);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "לא נבחרו אמנים למכרז");
                }
            }

            ModelState.AddModelError("", "לא הצלחנו לשמור את הנתונים");

            TempData["ViewData"] = ViewData;
            return RedirectToAction("Confirm", new { Id = au.AuctionId });
        }

        [Authorize(Roles = "SystemAdmin,Customer")]
        [AJAXValidateAntiForgeryToken]
        public ActionResult OpenNew(Auction NewAuction)
        {
            ActionResponse res = new ActionResponse();
            res.Succeeded = false; 
            if (ModelState.IsValid)
            {
                var CreateNewRes = this._actionSrv.Add(NewAuction);

                if (CreateNewRes.Success)
                {
                    res.Succeeded = true;
                }
            }
            
            return Json(res);
        }
    }
}