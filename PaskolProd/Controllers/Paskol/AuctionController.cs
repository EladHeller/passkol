using Microsoft.AspNet.Identity;
using Model;
using Model.Auctions;
using Model.Filters;
using Model.Helper;
using Newtonsoft.Json;
using PaskolProd.Areas.Admin.Controllers;
using PaskolProd.Authentication;
using PaskolProd.Controllers.Helper;
using PaskolProd.Models;
using PaskolProd.Models.Response;
using Services;
using Services.Messaging.Requests;
using Services.Messaging.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PaskolProd.Paskol.Controllers
{
    public class AuctionController : UserBaseController
    {
        const int RES_IN_PAGE = 25;
        private AuctionService _actionSrv;
        private MusicService _musicSrv;

        public AuctionController()
        {
            _actionSrv = ServiceLocator.GetService<AuctionService>();
            _musicSrv = ServiceLocator.GetService<MusicService>();
        }

        [Authorize(Roles = "SystemAdmin,Customer")]
        [AJAXValidateAntiForgeryToken]
        public ActionResult OpenNew(Auction NewAuction)
        {
            ActionResponse res = new ActionResponse();
            res.Succeeded = false;
            if (ModelState.IsValid)
            {
                // get customer from HttpcOntext
                var customer = User.GetUser();
                NewAuction.Customer = customer;
                NewAuction.AuctionStatus = AuctionStatus.ConfirmWating;
                var CreateNewRes = this._actionSrv.Add(NewAuction);

                if (CreateNewRes.Success)
                {
                    res.Succeeded = true;
                }
            }
            
            return Json(res);
        }

        [HttpPost]
        [Authorize(Roles = "SystemAdmin,Customer")]
        [AJAXValidateAntiForgeryToken]
        public string GetAuctionsForUser()
        {
            EntityAllResponse<AuctionListCustomerViewModel> AuctoinRes = 
                new EntityAllResponse<AuctionListCustomerViewModel>();

            var res = _actionSrv.GetAuctionsForCustomer(User.GetUser().Id);
            AuctoinRes.Entities = res.Entities.Select(a => new AuctionListCustomerViewModel()
            {
                Id = a.AuctionId,
                AuctionName = a.AuctionName,
                AuctionStatus = a.AuctionStatus.ToDescription(),
                CloseDate = a.CloseDate == null ? "" : a.CloseDate.Value.ToShortDateString(),
                OpenDate = a.OpenDate == null ? "" : a.OpenDate.Value.ToShortDateString(),
                OpenProjectDate = a.OpenProjectDate == null ? "" : a.OpenProjectDate.Value.ToShortDateString(),
                PickWinnerDate = a.PickWinnerDate == null ? "" : a.PickWinnerDate.Value.ToShortDateString(),
                SketchesCount = a.Sketches.Count
            }).ToList();
            AuctoinRes.Message = res.Message;
            AuctoinRes.Success = res.Success;
            return JsonConvert.SerializeObject(AuctoinRes);
        }
        
        [HttpPost]
        [Authorize(Roles = "SystemAdmin,Customer")]
        [AJAXValidateAntiForgeryToken]
        public string GetSketchesForAuction(string ID)
        {
            EntityAllResponse<SketchesListViewModel> AuctoinRes =
                new EntityAllResponse<SketchesListViewModel>();

            var res = _actionSrv.GetByID(ID);
            AuctoinRes.Entities = res.Entity.Sketches.Select(a => new SketchesListViewModel()
            {
                SketchID = a.ID,
                AuctionId = ID,
                ArtistId = a.Artist.User.Id,
                File = a.MP3File,
                ArtistName = a.Artist.User.UserName,
                Note = a.Comment,
                SketchesName = a.Name
            });

            AuctoinRes.Message = res.Message;
            AuctoinRes.Success = res.Success;
            return JsonConvert.SerializeObject(AuctoinRes);
        }
        
        [HttpPost]
        [Authorize(Roles = "SystemAdmin,Artist")]
        [AJAXValidateAntiForgeryToken]
        public string GetAuctionsForArtist()
        {
            EntityAllResponse<AuctionListArtistViewModel> AuctoinRes =
                new EntityAllResponse<AuctionListArtistViewModel>();

            string UserId = User.Identity.GetUserId();
            var res = _actionSrv.GetAuctionsForArtist(UserId);
            AuctoinRes.Entities = res.Entities.Select(a => new AuctionListArtistViewModel()
            {
                Id = a.AuctionId,
                AuctionName = a.AuctionName,
                AuctionStatus = setAuctionStatusForArtist(a.AuctionStatus),
                CloseDate = a.CloseDate.Value.ToShortDateString(),
                OpenDate = a.OpenDate.Value.ToShortDateString(),
                PickWinnerDate = a.PickWinnerDate.Value.ToShortDateString(),
                Won = a.WinnerArtist?.Id == UserId,
                SketchSent = a.Sketches?.Any(s => s.ArtistID == UserId)
            });
            AuctoinRes.Message = res.Message;
            AuctoinRes.Success = res.Success;
            return JsonConvert.SerializeObject(AuctoinRes);
        }
        
        [HttpPost]
        [Authorize(Roles = "Artist")]
        [AJAXValidateAntiForgeryToken]
        public void ParticipateInAuctionToggele(bool p)
        {
            string UserId = User.Identity.GetUserId();

            var user = UserManager.FindById(UserId);
            user.Artist.ParticipateInAuction = p;
            UserManager.Update(user);
        }

        [HttpPost]
        [Authorize(Roles = "Artist")]
        [AJAXValidateAntiForgeryToken]
        public string GetAuction(string id)
        {
            EntityResponse<AuctionRequirmentsViewModel> res =
                new EntityResponse<AuctionRequirmentsViewModel>();
            res.Success = false;

            var AuctionResponse = _actionSrv.GetByID(id);

            // if artist is participate with this auction
            if (AuctionResponse?.Entity?.Participants?.Any(p => p.Id == User.Identity.GetUserId()) == true)
            {
                res.Success = true;
                res.Entity = new AuctionRequirmentsViewModel()
                {
                    AuctionId = AuctionResponse.Entity.AuctionId,
                    Budget = AuctionResponse.Entity.AuctionBudget,
                    ContactMan = AuctionResponse.Entity.ContactName,
                    Email = AuctionResponse.Entity.ContactEmail,
                    MusicLength = AuctionResponse.Entity.MusicLength,
                    Notes = AuctionResponse.Entity.Notes,
                    Using = AuctionResponse.Entity.UsingName,
                    Phone = AuctionResponse.Entity.ContactPhone,
                    Open = AuctionResponse.Entity.OpenDate.Value.ToString("yyyy/MM/dd"),
                    End = AuctionResponse.Entity.CloseDate.Value.ToString("yyyy/MM/dd"),
                    MusicProperties = AuctionResponse.Entity.MusicProperties
                };
            }

            return JsonConvert.SerializeObject(res);
        }

        [HttpPost]
        [Authorize(Roles = "Artist")]
        [AJAXValidateAntiForgeryToken]
        public ActionResult UploadSketch(HttpPostedFileBase WAVUpload,
            HttpPostedFileBase MP3Upload, string ArtistNote, string AuctionId, string name)
        {
            if (ModelState.IsValid)
            {
                string ArtistId = User.Identity.GetUserId();
                var auction = this._actionSrv.GetByID(AuctionId).Entity;

                if (auction != null)
                {
                    // Create music obg
                    var sketch = new Sketch()
                    {
                        Name = name,
                        ArtistID = ArtistId,
                        Comment = ArtistNote,
                        CreateDate = DateTime.Now
                    };

                    // Convert music to File
                    Model.File mp3File = FileHelper.
                                   ConvertPostedFileToFile(MP3Upload, 0, ArtistId, null);
                    Model.File wavFile = FileHelper.
                        ConvertPostedFileToFile(WAVUpload, 0, ArtistId, null);
                    
                    // Add music to db and to file system
                    var res = _musicSrv.AddSketchToAuction(auction, sketch, wavFile, mp3File);
                }
            }
            return Redirect("~/Artist/OriginalMusic");
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        [AJAXValidateAntiForgeryToken]
        public bool CanBuy(string ID)
        {
            if (!string.IsNullOrEmpty(ID))
            {
                string CustomerId = User.Identity.GetUserId();
                var auction = this._actionSrv.GetByID(ID).Entity;

                return auction.CloseDate < DateTime.Now && 
                    auction.WinnerArtist == null && auction.AuctionStatus != AuctionStatus.close && 
                    auction.AuctionStatus != AuctionStatus.CloseWating;
            }

            return false;
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        [AJAXValidateAntiForgeryToken]
        public bool BuySketch(string AuctionID, string SketchID)
        {
            if (!string.IsNullOrEmpty(AuctionID) && !string.IsNullOrEmpty(SketchID) && CanBuy(AuctionID))
            {
                return this._actionSrv.BuySketch(AuctionID, SketchID).Success;
            }

            return false;
        }

        private string setAuctionStatusForArtist(AuctionStatus acs)
        {
            string name = "";
            switch (acs)
            {
                case AuctionStatus.Open:
                    name = "פתוח";
                    break;
                case AuctionStatus.close:
                    name = "סגור";
                    break;
            }
            return name;
        }
    }
}