using Repository.UnitOfWork;
using Repository.Interfaces;
using Services.Messaging.Responses;
using System;
using Services.Messaging.Requests;
using Repository;
using Model.Users;
using Model.Auctions;
using System.Collections.Generic;
using System.Linq;
using Model;

namespace Services
{
    public class AuctionService : GenericServiceUOW<Auction, string>
    {
        private IAuctionRepository _auctionRep;
        private IUsersRepository _userRep;
        private PaskolEmailService _emailService;
        private PermissionCategoryService _pcService;

        public AuctionService(PermissionCategoryService pcService,
            PaskolEmailService emailService,
            IAuctionRepository rep, IUsersRepository userRep, IUnitOfWork uow) :base(rep,uow)
        {
            this._pcService = pcService;
            this._userRep = userRep;
            this._auctionRep = rep;
            _emailService = emailService;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public ResponseBase closeAuction(Auction a)
        {
            ResponseBase res = new ResponseBase();

            try
            {
                a.AuctionStatus = AuctionStatus.close;
                this._auctionRep.Update(a);
                this._uow.Commit();

                //
                _emailService.CloseAuction(a);

                res.Success = true;
            }
            catch (Exception e)
            {
                res.Message = e.Message;
                res.Success = false;
            }

            return res;
        }
        
        public ResponseBase ConfirmAuction(Auction au)
        {
            ResponseBase res = new ResponseBase();

            try
            {
                // Retrive the action to update from db
                var AuctionToUpdate = this._auctionRep.GetByID(au.AuctionId);
                if (AuctionToUpdate != null)
                {
                    // Get the participants to save with action
                    var ParticipantsList = new List<Artist>();
                    
                    foreach (PaskolUser user in this._userRep.GetAllBy(UserType.Artist))
                    {
                        foreach (var InUser in au.Participants)
                        {
                            if (user.UserName == InUser.User.UserName)
                            {
                                ParticipantsList.Add(user.Artist);
                            }
                        }
                    }

                    AuctionToUpdate.AuctionName = au.AuctionName;
                    AuctionToUpdate.UsingName = au.UsingName;
                    AuctionToUpdate.OpenDate = DateTime.Now;//open date is the time auction was confirmed
                    AuctionToUpdate.CloseDate = au.CloseDate;
                    AuctionToUpdate.PickWinnerDate = au.PickWinnerDate;
                    AuctionToUpdate.MusicProperties = au.MusicProperties;
                    AuctionToUpdate.MusicLength = au.MusicLength;
                    AuctionToUpdate.AuctionBudget = au.AuctionBudget;
                    AuctionToUpdate.ContactName = au.ContactName;
                    AuctionToUpdate.ContactPhone = au.ContactPhone;
                    AuctionToUpdate.ContactEmail = au.ContactEmail;
                    AuctionToUpdate.Notes = au.Notes;
                    AuctionToUpdate.Participants = ParticipantsList;
                    AuctionToUpdate.AuctionStatus = AuctionStatus.Open;

                    this._auctionRep.Update(AuctionToUpdate);
                    this._uow.Commit();

                    // Invite participants to participate in auction
                    _emailService.InviteArtistToAuction(AuctionToUpdate);

                    res.Success = true;
                }
                else
                {
                    res.Success = false;
                    res.Message = "לא הצלחנו לאתר את המכרז לעדכון";
                }
            }
            catch (Exception e)
            {
                res.Message = e.Message + e.InnerException?.Message + e.InnerException?.InnerException?.Message;

                
                res.Success = false;
            }

            return res;
        }

        public PagingResponse<Auction> Get(PagingRequest pageRequest, AuctionSort sortOrder, string UserId = null)
        {
            PagingResponse<Auction> res = new PagingResponse<Auction>();

            try
            {
                var repositoryRes = (this._rep as AuctionRepository).Get(pageRequest.ResultInPage,
                    (pageRequest.Page - 1) * pageRequest.ResultInPage, sortOrder,UserId);

                res.Entities = repositoryRes.Entities;
                res.TotalResults = repositoryRes.TotalResults;
                res.ResultsInPage = pageRequest.ResultInPage;
                res.CurrPage = pageRequest.Page;
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Success = false;
                res.Message = e.ToString();
            }

            return res;
        }

        public EntityAllResponse<Auction> GetAuctionsForCustomer(string UserID)
        {
            EntityAllResponse<Auction> res = new EntityAllResponse<Auction>();

            try
            {
                res.Entities = this._auctionRep.GetAuctionsForCustomer(UserID);
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Success = false;
                res.Message = e.Message;
            }

            return res;
        }

        public EntityAllResponse<Auction> GetAuctionsForArtist(string UserID)
        {
            EntityAllResponse<Auction> res = new EntityAllResponse<Auction>();

            try
            {
                res.Entities = this._auctionRep.GetAuctionsForArtist(UserID);
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Success = false;
                res.Message = e.Message;
            }

            return res;
        }
        

        public ResponseBase BuySketch(string auctionID, string sketchId)
        {
            ResponseBase res = new ResponseBase();
            res.Success = false;
            try
            {
                var a = _rep.GetByID(auctionID);
                if (a != null)
                {
                    Sketch sketch = a.Sketches.SingleOrDefault(s => s.ID == sketchId);
                    if (sketch != null)
                    {
                        a.AuctionStatus = AuctionStatus.CloseWating;
                        sketch.Purchased = true;

                        _auctionRep.Update(a);
                        _uow.Commit();
                        res.Success = true;
                    }
                    else
                    {
                        res.Message = "לא מצאנו את הסקיצה";
                    }
                }
                else
                {
                    res.Message = "לא מצאנו את המכרז";
                }
               
            }
            catch (Exception e)
            {
                res.Message = e.Message;
            }

            return res;
        }
    }
}
