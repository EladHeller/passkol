using Microsoft.AspNet.Identity;
using Model.Confirm;
using Model.Filters;
using Model.PriceModel;
using PaskolProd.Authentication;
using PaskolProd.Models;
using PasskolProd.Models;
using Services;
using Services.Messaging.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Model.Controllers.Paskol
{
   
    public class PurchaseController : Controller
    {
        public PermissionCategoryService _permCatSrv { get; set; }
        public PermissionService _permSrv { get; set; }
        public MusicService _mscSrv { get; set; }
        public PurchaseService _prchSrv { get; set; }
        public ConfirmationService _cnfrmSrv { get; set; }

        public PurchaseController()
        {
            _permCatSrv = ServiceLocator.GetService<PermissionCategoryService>();
            _mscSrv = ServiceLocator.GetService<MusicService>();
            _permSrv = ServiceLocator.GetService<PermissionService>();
            _cnfrmSrv = ServiceLocator.GetService<ConfirmationService>();
            _prchSrv = ServiceLocator.GetService<PurchaseService>();
        }


        [HttpPost]
        public ActionResult PermissionsCategory()
        {
            var res = _permCatSrv.GetAll();
            IEnumerable<PermissionCategoryViewModel> categories = null;
            if (res.Success)
            {
                categories = res.Entities.Select(prm => new PermissionCategoryViewModel(prm));
            }
            return Json(new { Success = res.Success, Categories = categories, Message = res.Message });
        }

        [HttpPost]
        public ActionResult PermissionCategoryDetails(Guid id)
        {
            var res = _permCatSrv.GetByID(id, PermissionNavigationOptions.IncludeAll);

            PermissionCategoryViewModel category = null;
            if (res.Success)
            {
                category = new PermissionCategoryViewModel(res.Entity, true);
            }
            
            return Json(new { Success = res.Success, Category = category, Message = res.Message });
        }

        [HttpPost]
        [AJAXValidateAntiForgeryToken]
        public ActionResult CheckPermissionCost(IEnumerable<Guid> valueIds, int musicId, Guid categoryId)
        {
            var res = _permCatSrv.CheckPermissionCost(valueIds, musicId, categoryId);

            return Json(new { Success = res.Success, Cost = res.Entity.Cost });
        }

        [HttpPost]
        [AJAXValidateAntiForgeryToken]
        public ActionResult PhonePurchase(IEnumerable<Guid> valueIds, int musicId, Guid categoryId)
        {
            var res = _permCatSrv.CheckPermissionCost(valueIds, musicId, categoryId);
            bool isSuccess = res.Success;

            if (res.Success && res.Entity.Cost == 0)
            {
                var purchase = new Purchase.Purchase();
                purchase.Customer = User.GetUser();
                purchase.Music = res.Entity.Music;
                purchase.PurchaseCost = 0;
                purchase.PurchaseDate = DateTime.Now;
                purchase.PurchaseStatus = Purchase.PurchaseStatus.NotPaidBycustomer;
                
                if (res.Entity.IsExistPermission)
                {
                    var addRes = 
                        _permCatSrv.GetOrAdd(res.Entity.Permission.PropertyValues.Select(val => val.ID), categoryId);
                    isSuccess = addRes.Success;
                    if (isSuccess)
                    {
                        purchase.Permision = addRes.Entity;
                    }
                }
                else
                {
                    purchase.Permision = res.Entity.Permission;
                }

                if (isSuccess)
                {
                    isSuccess = _prchSrv.Add(purchase).Success;
                }
                if (isSuccess)
                {
                    var confirm = new Confirmation()
                    {
                        ConfirmType = ConfirmType.PhonePurchase,
                        EntityId = purchase.PurchaseId,
                        Name = purchase.Music.HebrewName,
                        DateUpdate = DateTime.Now
                    };

                    isSuccess = _cnfrmSrv.Add(confirm).Success;
                }
            }

            return Json(isSuccess);
        }

        [HttpPost]
        [Authorize(Roles ="Customer")]
        [AJAXValidateAntiForgeryToken]
        public JsonResult GetCustomerReports(int Index, SortPurchase sort)
        {
            const int RES_In_PAGE = 25;
            var res = this._prchSrv.GetByCustomerId(User.Identity.GetUserId());
            var entities = res.Entities;
            var totalRes = res.Entities.Count();

            switch (sort)
            {
                case SortPurchase.OldToNew:
                    entities = res.Entities.OrderBy(p => p.PurchaseDate)
                        .Take(RES_In_PAGE)
                        .Skip((Index - 1) * RES_In_PAGE)
                        .ToArray();
                        break;
                case SortPurchase.NewToOld:
                    entities = res.Entities.OrderByDescending(p => p.PurchaseDate)
                       .Take(RES_In_PAGE)
                       .Skip((Index - 1) * RES_In_PAGE)
                       .ToArray();
                    break;
            }

            return Json(new { TotalPages = totalRes, entities = 
                entities.Select(p => new PurchaseReportVM
                {
                    ID = p.PurchaseId,
                    DatePurchased = p.PurchaseDate.ToString("HH:mm yyyy/MM/dd"),
                    MusicName = p.Music.HebrewName,
                    ArtistName = p.Music.Artist.User.UserName,
                    PermissionToString = string.Join(",", p.Permision.PropertyValues.Where(pv => !pv.PermissionProperty.IsCostLevel).Select(pv => pv.Name)),
                    Price = p.PurchaseCost,
                    AgreementFileID = p.PurchaseAgreement?.FileId,
                    ArtistID = p.Music.ArtistID,
                    Musicfile = p.Music.WAVFile
                })
            });
        }

        [HttpPost]
        [Authorize(Roles = "ArtistAgent")]
        [AJAXValidateAntiForgeryToken]
        public JsonResult GetArtistAgntReports(int Index, SortPurchase sort)
        {
            var res = this._prchSrv.GetByArtistAgentId(User.Identity.GetUserId());
            return GetReports(res, Index, sort);
        }

        [HttpPost]
        [Authorize(Roles = "Artist")]
        [AJAXValidateAntiForgeryToken]
        public JsonResult GetArtistReports(int Index, SortPurchase sort)
        {
            var res = this._prchSrv.GetByArtistId(User.Identity.GetUserId());
            return GetReports(res,Index,sort);
        }

        private JsonResult GetReports(EntityAllResponse<Purchase.Purchase> res, int Index, SortPurchase sort)
        {
            const int RES_In_PAGE = 25;

            var entities = res.Entities;
            var totalRes = res.Entities.Count();

            switch (sort)
            {
                case SortPurchase.OldToNew:
                    entities = res.Entities.OrderBy(p => p.PurchaseDate)
                        .Take(RES_In_PAGE)
                        .Skip((Index - 1) * RES_In_PAGE)
                        .ToArray();
                    break;
                case SortPurchase.NewToOld:
                    entities = res.Entities.OrderByDescending(p => p.PurchaseDate)
                       .Take(RES_In_PAGE)
                       .Skip((Index - 1) * RES_In_PAGE)
                       .ToArray();
                    break;
            }

            return Json(new
            {
                TotalPages = totalRes,
                entities =
                entities.Select(p => new PurchaseReportVM
                {
                    ID = p.PurchaseId,
                    DatePurchased = p.PurchaseDate.ToString("HH:mm yyyy/MM/dd"),
                    MusicName = p.Music.HebrewName,
                    ArtistName = p.Music.Artist.User.UserName,
                    PermissionToString = string.Join(",", p.Permision.PropertyValues.Where(pv => !pv.PermissionProperty.IsCostLevel).Select(pv => pv.Name)),
                    Price = p.Permision.PermissionCost,
                    AgreementFileID = p.PurchaseAgreement?.FileId,
                    ArtistID = p.Music.ArtistID
                })
            });
        }
    }
}