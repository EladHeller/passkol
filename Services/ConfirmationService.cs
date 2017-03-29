using Repository.UnitOfWork;
using Repository.Interfaces;
using Services.Messaging.Responses;
using System;
using Services.Messaging.Requests;
using Repository;
using Model.Users;
using Model.Confirm;
using Model.Purchase;
using System.Net.Http;
using System.Linq;
using Model;
using Services.PDFService;
using System.Collections.Generic;
using Infrastructure.Helper;
using System.Threading.Tasks;
using Infrastructure.Configuration;
using Infrastructure.Configuration;
using System.IO;

namespace Services
{
    public class ConfirmationService : GenericServiceUOW<Confirmation, int>
    {
        private IPurchaseRepository _purchasseRep;
        private IMusicsRepository _MusicRep;
        private PaskolEmailService _emailService;
        private PDFServiceSoapClient _pdfService;
        private MusicService _MusicSrv;

        public ConfirmationService(IConfirmationRepository rep, 
            IPurchaseRepository purchasseRep, IMusicsRepository MusicRep, IUnitOfWork uow) :base(rep,uow)
        {
            this._MusicRep = MusicRep;
            this._purchasseRep = purchasseRep;
            _emailService = ServiceLocator.GetService<PaskolEmailService>();
            _pdfService = ServiceLocator.GetService<PDFServiceSoapClient>();
            _MusicSrv = ServiceLocator.GetService<MusicService>();
        }

        public EntityResponse<Confirmation> GetByEntityId(string EntityId)
        {
            var res = new EntityResponse<Confirmation>();
            res.Success = false;

            try
            {
                res.Entity = ((ConfirmationRepository)this._rep)
                    .GetByEntityId(EntityId);
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Message = e.ToString();
            }
            return res;
        }

        public ResponseBase Delete(string EntityId)
        {
            string baseError = "לא הצלחנו למחוק מרשימת ממתינים לאישור";

            ResponseBase res = new ResponseBase();
            try
            {
                Confirmation conf = ((ConfirmationRepository)this._rep)
                    .GetByEntityId(EntityId);

                if (conf != null)
                {
                    this._rep.Delete(conf);
                    this._uow.Commit();
                    res.Success = true;
                }
                else
                {
                    res.Success = false;
                    res.Message = baseError;
                }
            }
            catch (Exception e)
            {
                res.Message = e.Message + " " + baseError;
                res.Success = false;
            }

            return res;
        }

        public PagingResponse<Confirmation> GetBySearch(IList<ConfirmType> Types, PagingRequest pageRequest, ConfirmSorting sortOrder)
        {
            PagingResponse<Confirmation> res = new PagingResponse<Confirmation>();

            try
            {
                var repositoryRes = (this._rep as ConfirmationRepository).GetBySearch(Types,
                    pageRequest.ResultInPage,
                    (pageRequest.Page - 1) * pageRequest.ResultInPage, sortOrder);

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

        public ResponseBase ConfirmPhonePurchase(Purchase ConfPurchase, ConfirmTypeAction confirmAction)
        {
            ResponseBase res = new ResponseBase();

            try
            {
                var purchase = this._purchasseRep.GetByID(ConfPurchase.PurchaseId);
                
                if (purchase != null)
                {

                    // delete from confirmation table
                    Confirmation conf = ((ConfirmationRepository)this._rep)
                        .GetByEntityId(ConfPurchase.PurchaseId.ToString());

                    if (conf != null)
                    {
                        switch (confirmAction)
                        {
                            case ConfirmTypeAction.Ok:
                                var file = new Model.File()
                                {
                                    FileType = FileType.PDF,
                                    suffix = ".pdf",
                                    UserId = ConfPurchase.Customer.Id,
                                };
                                // save that paid by customer but not paid to artist
                                purchase.PurchaseStatus = PurchaseStatus.NotPaidToArtist;
                                purchase.Note = " שולם.. " +  ConfPurchase.Note;
                                purchase.PurchaseCost = ConfPurchase.PurchaseCost;
                                purchase.CustomerReference = ConfPurchase.CustomerReference;
                                purchase.PurchaseAgreement = file;
                                this._purchasseRep.Update(purchase);
                                this._rep.Delete(conf);
                                this._uow.Commit();

                                string pathToDownload = string.Format(CommonHelper.GetBaseUrl() +
                                    "/File/GetFile?FileId={0}&UserId={1}&FileType={2}&version={3}&suffix={4}&FileName={5}"
                                    , purchase.Music.WAVFileID, purchase.Music.ArtistID,FileType.Music,
                                    purchase.Music.WAVFile.version, purchase.Music.WAVFile.suffix, purchase.Music.HebrewName);

                                string permissions = 
                                    string.Join(", ", purchase.Permision.PropertyValues
                                        .Where(pv=>!pv.PermissionProperty.IsCostLevel)
                                        .Select(pv =>pv.PermissionProperty.Name + " - " + pv.Name));
                                
                                // Create Report
                                _pdfService.PurchaseAgreement(
                                    WebConf.FSBaseRoute,
                                    Path.GetFileName(file.RelativePath),
                                    purchase.Customer.Id,
                                    DateTime.Now,
                                    purchase.Customer.UserName,
                                    purchase.Customer.Email,
                                    purchase.Music.HebrewName,
                                    purchase.Music.MusicWriter,
                                    purchase.Music.MusicComposer,
                                    purchase.Music.MusicPerformer,
                                    permissions,
                                    purchase.PurchaseCost.HasValue ? purchase.PurchaseCost.Value : 0,
                                    purchase.CustomerReference,
                                    purchase.Music.Exceptions);
                                
                                // Send email
                                _emailService.AlertArtistPurchaseSuccess(purchase.Music.Artist.User.Email);
                                _emailService.AlertBuyerPurchaseSuccess(purchase.Customer.Email, pathToDownload);
                               
                                break;
                            case ConfirmTypeAction.Decline:
                                purchase.Note = " בוטל " + ConfPurchase.Note;
                                this._purchasseRep.Update(purchase);
                                this._uow.Commit();
                                break;
                        }
                        res.Success = true;
                    }
                }
                else
                {
                    res.Message = "הרכישה המבוקשת לעדכון לא נמצאה";
                    res.Success = false;
                }
                
            }
            catch (Exception e)
            {
                res.Message = e.Message;
                res.Success = false;
            }
            
            return res;
        }

        public ResponseBase ConfirmNewMusic(Music music, Model.File WAVUpload,
            Model.File MP3Upload, ConfirmTypeAction confirmAction, string DeclineReason)
        {
            ResponseBase res = new ResponseBase();

            try
            {   
                // delete from confirmation table
                Confirmation conf = ((ConfirmationRepository)this._rep)
                    .GetByEntityId(music.ID.ToString());

                if (conf != null)
                {
                    this._rep.Delete(conf);

                    switch (confirmAction)
                    {
                        case ConfirmTypeAction.Ok:
                            music.Status = MusicActiveStatus.Public;

                            // update async artist report by updating his pdf file
                            _pdfService.ArtistAddMusicAsync(DateTime.Now, music.HebrewName,
                                music.MusicComposer,music.MusicPerformer,music.Exceptions,music.MusicWriter,
                                music.ArtistID,WebConf.FSBaseRoute);
                            break;
                        case ConfirmTypeAction.Decline:
                            _emailService.DeclineNewUpdateMusic(music.Artist.User.Email,DeclineReason,music.HebrewName);
                            break;
                        case ConfirmTypeAction.Block:
                            music.Status = MusicActiveStatus.Blocked;
                            break;
                    }

                    var resUpDateOK = _MusicSrv.Update(music, music.ID, WAVUpload, MP3Upload);
                    this._uow.Commit();
                    res.Success = true;
                }
                else
                {
                    res.Message = "לא הצלחנו לאתר נתונים";
                    res.Success = false;
                }

            }
            catch (Exception e)
            {
                res.Message = e.Message;
                res.Success = false;
            }

            return res;
        }

        public ResponseBase ConfirmEditedMusic(Music music, Model.File WAVUpload,
        Model.File MP3Upload, ConfirmTypeAction confirmAction, string DeclineReason)
        {
            ResponseBase res = new ResponseBase();

            try
            {
                // delete from confirmation table
                Confirmation conf = ((ConfirmationRepository)this._rep)
                    .GetByEntityId(music.ID.ToString());

                if (conf != null)
                {
                    this._rep.Delete(conf);

                    switch (confirmAction)
                    {
                        case ConfirmTypeAction.Ok:
                            // update source music by music
                            music.Status = MusicActiveStatus.Public;
                            if(_MusicSrv.Update(music, music.SourceMusicId.Value, WAVUpload, MP3Upload).Success)
                            {
                                // Delete temp music
                                _MusicSrv.DeleteBy(music.ID);
                            }
                            break;
                        case ConfirmTypeAction.Decline:
                            _MusicSrv.Update(music, music.ID, WAVUpload, MP3Upload);
                            _emailService.DeclineNewUpdateMusic(music.Artist.User.Email, DeclineReason,music.HebrewName);
                            break;
                        case ConfirmTypeAction.Block:
                            // Delete from temp
                            _MusicSrv.DeleteBy(music.ID);

                            // Set the source music as blocked
                            var SourceMusic = _MusicRep.GetByID(music.SourceMusicId.Value);
                            SourceMusic.Status = MusicActiveStatus.Blocked;
                            _MusicRep.Update(SourceMusic);
                            break;
                    }
                    
                    this._uow.Commit();
                    res.Success = true;
                }
                else
                {
                    res.Message = "לא הצלחנו לאתר נתונים";
                    res.Success = false;
                }

            }
            catch (Exception e)
            {
                res.Message = e.Message;
                res.Success = false;
            }

            return res;
        }
    }
}
