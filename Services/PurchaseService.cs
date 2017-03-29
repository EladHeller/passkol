using Repository.UnitOfWork;
using Repository.Interfaces;
using Services.Messaging.Responses;
using System;
using Services.Messaging.Requests;
using Repository;
using Model.Users;
using Model.Confirm;
using Model.Purchase;
using Model;
using System.Linq;
using Infrastructure.Helper;
using Services.PDFService;
using Infrastructure.Configuration;
using System.IO;

namespace Services
{
    public class PurchaseService : GenericServiceUOW<Purchase, string>
    {
        private PaskolEmailService _emailSrv;
        private IUsersRepository _usersRep;
        private IMusicsRepository _musicRep;
        private IPermmsionRepository _permissionRep;
        private PDFServiceSoapClient _pdfService;

        public PurchaseService(PaskolEmailService emailSrv, 
            IPurchaseRepository rep, IUsersRepository usersRep, IMusicsRepository musicRep,
            IPermmsionRepository perRep, IUnitOfWork uow) : base(rep, uow)
        {
            this._emailSrv = emailSrv;
            this._usersRep = usersRep;
            this._musicRep = musicRep;
            this._permissionRep = perRep;
            _pdfService = ServiceLocator.GetService<PDFServiceSoapClient>();
        }

        public ResponseBase SaveNewPurchase(int? MusicId, PaskolUser Buyer, double Total, Guid permId)
        {
            ResponseBase res = new ResponseBase();

            try
            {
                Music music = null;

                if (MusicId != null && Buyer != null)
                {
                    music = _musicRep.GetByID(MusicId.Value);

                    if (music != null)
                    {
                        var file = new Model.File()
                        {
                            FileType = FileType.PDF,
                            suffix = ".pdf",
                            UserId = Buyer.Id,
                        };
                        Purchase newPurchase = new Purchase();
                        newPurchase.Customer = Buyer;
                        newPurchase.Music = music;
                        newPurchase.PurchaseCost = Total;
                        newPurchase.PurchaseStatus = PurchaseStatus.NotPaidToArtist;
                        newPurchase.PurchaseDate = DateTime.Now;
                        newPurchase.Permision = _permissionRep.GetByID(permId);
                        newPurchase.CustomerReference = "";//TODO
                        newPurchase.PurchaseAgreement = file;
                        this._rep.Add(newPurchase);
                        this._uow.Commit();

                        string pathToDownload = string.Format(CommonHelper.GetBaseUrl() +
                                    "/File/GetFile?FileId={0}&UserId={1}&FileType={2}&version={3}&suffix={4}&FileName={5}"
                                    , music.WAVFileID, music.ArtistID, FileType.Music,
                                    music.WAVFile.version, music.WAVFile.suffix, music.HebrewName);
                        string permissions =
                                    string.Join(", ", newPurchase.Permision.PropertyValues
                                        .Where(pv => !pv.PermissionProperty.IsCostLevel)
                                        .Select(pv => pv.PermissionProperty.Name + " - " + pv.Name));
                        // Create Report
                        _pdfService.PurchaseAgreement(
                            WebConf.FSBaseRoute,
                            Path.GetFileName(file.RelativePath),
                            Buyer.Id,
                            DateTime.Now,
                            Buyer.UserName,
                            Buyer.Email,
                            music.HebrewName,
                            music.MusicWriter,
                            music.MusicComposer,
                            music.MusicPerformer,
                            permissions,
                            newPurchase.PurchaseCost.HasValue ? newPurchase.PurchaseCost.Value : 0,
                            newPurchase.CustomerReference,
                            newPurchase.Music.Exceptions);

                        // notify artist and customer
                        this._emailSrv.AlertArtistPurchaseSuccess(newPurchase.Music.Artist.User.Email);
                        this._emailSrv.AlertBuyerPurchaseSuccess(newPurchase.Customer.Email, pathToDownload);
                        res.Success = true;
                    }
                }

                if (music == null || Buyer == null)
                {
                    res.Success = false;
                    res.Message = "לא הצלחנו לזהות לקוח או מוזיקה";
                }
            }
            catch (Exception e)
            {
                res.Message = e.Message;
                res.Success = false;
            }
            return res;
        }

        public EntityResponse<string> GetPurchaseHTML(int musicID, PaskolUser buyer, int cost, string reference, string permission)
        {
            var res = new EntityResponse<string>();

            try
            {
                Music music = _musicRep.GetByID(musicID);

                res.Entity = 
                    _pdfService.GetTempPurchaseHtml(
                            DateTime.Now,
                            buyer.UserName,
                            buyer.Email,
                            music.HebrewName,
                            music.MusicWriter,
                            music.MusicComposer,
                            music.MusicPerformer,
                            permission,
                            cost,
                            reference,
                            music.Exceptions);
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Success = false;
                res.Message = e.ToString();
            }

            return res;
        }

        public ResponseBase UpdatePaidToArtist(Purchase pToUpdate)
        {
            ResponseBase res = new ResponseBase();

            try
            {
                var dbP = this._rep.GetByID(pToUpdate.PurchaseId);

                if (dbP != null)
                {
                    dbP.ArtistEarn = pToUpdate.ArtistEarn;
                    dbP.PaidToArtistReference = pToUpdate.PaidToArtistReference;
                    dbP.PaidToArtistNote = pToUpdate.PaidToArtistNote;
                    dbP.PurchaseStatus = PurchaseStatus.PaidToArtist;

                    this._rep.Update(dbP);
                    // Update reports TODO
                    this._uow.Commit();
                }
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Message = e.Message;
                res.Success = false;
            }
            return res;
        }

        public EntityAllResponse<Purchase> GetByCustomerId(string Id)
        {
            EntityAllResponse<Purchase> res = new EntityAllResponse<Purchase>();
            try
            {
                res.Entities = ((IPurchaseRepository)this._rep)
                    .GetByCustomerId(Id);
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Message = e.Message;
                res.Success = false;
                throw;
            }
            return res;
        }

        public EntityAllResponse<Purchase> GetByArtistId(string Id)
        {
            EntityAllResponse<Purchase> res = new EntityAllResponse<Purchase>();

            try
            {
                res.Entities = ((IPurchaseRepository)this._rep)
                    .GetByArtistId(Id);
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Message = e.Message;
                res.Success = false;
                throw;
            }
            return res;
        }

        public EntityAllResponse<Purchase> GetByArtistAgentId(string Id)
        {
            EntityAllResponse<Purchase> res = new EntityAllResponse<Purchase>();

            try
            {
                // Get ArtistAgent
                PaskolUser artAgnt = _usersRep.GetByID(Id);
                if (artAgnt?.ArtistAgent != null)
                {
                    res.Entities = ((IPurchaseRepository)this._rep)
                   .GetByArtistAgntId(artAgnt.ArtistAgent.Artists.Select(u => u.User.Id).ToArray());
                    res.Success = true;
                }
                else
                {
                    res.Message = "לא מצאנו את מנהל האמנים";
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

        public EntityAllResponse<Purchase> GetByMusicId(int Id)
        {
            EntityAllResponse<Purchase> res = new EntityAllResponse<Purchase>();

            try
            {
                res.Entities = ((IPurchaseRepository)this._rep)
                    .GetByMusicId(Id);
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Message = e.Message;
                res.Success = false;
            }
            return res;
        }

        public PagingResponse<Purchase> Get(PagingRequest pageRequest, DateTime s, 
            DateTime e, PurchaseStatus? ps, bool WithoutPhonePurchase)
        {
            PagingResponse<Purchase> res = new PagingResponse<Purchase>();

            try
            {
                var repositoryRes = (this._rep as IPurchaseRepository).Get(pageRequest.ResultInPage,
                    (pageRequest.Page - 1) * pageRequest.ResultInPage,s,e, ps, WithoutPhonePurchase);

                res.Entities = repositoryRes.Entities;
                res.TotalResults = repositoryRes.TotalResults;
                res.ResultsInPage = pageRequest.ResultInPage;
                res.CurrPage = pageRequest.Page;
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = ex.ToString();
            }

            return res;
        }

        public EntityAllResponse<Purchase> GetBetweenDates(DateTime s, DateTime e, bool WithoutPhonePurchase)
        {
            EntityAllResponse<Purchase> res = new EntityAllResponse<Purchase>();

            try
            {
                res.Entities = ((IPurchaseRepository)this._rep)
                    .GetByDates(s, e);

                if (WithoutPhonePurchase)
                {
                    res.Entities = res.Entities
                        .Where(p => p.PurchaseStatus != PurchaseStatus.NotPaidBycustomer);
                }
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Success = false;
                throw;
            }
            return res;
        }
    }
}
