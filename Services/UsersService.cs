using Repository.UnitOfWork;
using Repository.Interfaces;
using Services.Messaging.Responses;
using System;
using Model.Users;
using System.Linq;
using Infrastructure.Helper;

namespace Services
{
    public class UsersService : GenericServiceUOW<PaskolUser, string>
    {
        private PaskolEmailService _emailSrv;
        public UsersService(IUsersRepository rep, PaskolEmailService emailSrv, IUnitOfWork uow) :base(rep,uow)
        {
            _emailSrv = emailSrv;
        }

        public EntityResponse<PaskolUser> GetByName(string name)
        {
            EntityResponse<PaskolUser> res = new EntityResponse<PaskolUser>();
            try
            {
                res.Entity = ((IUsersRepository)this._rep).GetByName(name);
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Success = false;
            }
            return res;
        }

        public EntityResponse<PaskolUser> GetArtistByUrlPage(string urlPage)
        {
            var res = new EntityResponse<PaskolUser>();

            try
            {
                res.Entity = (_rep as IUsersRepository).GetArtistByPageUrl(urlPage);
                res.Success = res.Entity != null;
                if (!res.Success)
                {
                    res.Message = "Artist not found";
                }
            }
            catch (Exception e)
            {
                res.Success = false;
                res.Message = e.ToString();
            }

            return res;
        }
        
        public ResponseBase AgencyRemovedBy(bool byArtist, string ArtistAgentId, string ArtistId)
        {
            ResponseBase res = new ResponseBase();
            res.Success = false;
            if (!string.IsNullOrEmpty(ArtistAgentId) && !string.IsNullOrEmpty(ArtistId))
            {
                var ArtistAgent = this._rep.GetByID(ArtistAgentId);

                if (ArtistAgent != null)
                {
                    try
                    {
                        var Artist = ArtistAgent?.ArtistAgent?.Artists
                            .FirstOrDefault(a => a.User.Id == ArtistId);

                        if (Artist != null)
                        {
                            if (byArtist)
                            {
                                Artist.RelConfByArtist = false;
                            }
                            else
                            {
                                Artist.RelConfByArtistAgent = false;
                            }
                            
                            // confirm consistent of relationship
                            _consistArtistAgentRel(Artist);
                            _rep.Update(Artist.User);
                            _uow.Commit();

                            res.Success = true;
                        }
                    }
                    catch (Exception e)
                    {
                        res.Message = e.Message;
                        res.Success = false;
                    }
                }
            }

            return res;
        }
        
        public ResponseBase AgencyConfirmedBy(bool byArtist, string ArtistAgentId, string ArtistId)
        {
            ResponseBase res = new ResponseBase();
            
            if (!string.IsNullOrEmpty(ArtistAgentId) && !string.IsNullOrEmpty(ArtistId))
            {
                var artist = _rep.GetByID(ArtistId);
                var artistAgnt = _rep.GetByID(ArtistAgentId);
                res = _agencyConfirmedBy(byArtist, artistAgnt, artist);
            }

            return res;
        }

        public ResponseBase AgencyConfirmedViaMailBy(bool byArtist, string ArtistAgentId, string Email)
        {
            ResponseBase res = new ResponseBase();
            PaskolUser artistAgnt = null;
            PaskolUser artist = null;
            if (!string.IsNullOrEmpty(ArtistAgentId) && !string.IsNullOrEmpty(Email))
            {
                if (byArtist)
                {
                    artistAgnt = (_rep as IUsersRepository).GetByEmail(Email);
                    artist = _rep.GetByID(ArtistAgentId);
                }
                else
                {
                    artist = (_rep as IUsersRepository).GetByEmail(Email);
                    artistAgnt = _rep.GetByID(ArtistAgentId);
                }
               
                res = _agencyConfirmedBy(byArtist, artistAgnt, artist);
            }

            return res;
        }

        private ResponseBase _agencyConfirmedBy(bool byArtist, PaskolUser ArtistAgent, PaskolUser Artist)
        {
            ResponseBase res = new ResponseBase();
            res.Success = false;

            try
            {
                if (Artist != null && ArtistAgent != null)
                {
                    // Set agent artist relationship
                    Artist.Artist.ArtistAgent = ArtistAgent.ArtistAgent;
                    var baseUrl = CommonHelper.GetBaseUrl();
                    string confirmUrl = "";
                    string removeUrl = "";
                    if (byArtist)
                    {
                        confirmUrl = baseUrl + "/ArtistAgent/PersonalManagment";
                        removeUrl = confirmUrl;
                        Artist.Artist.RelConfByArtist = true;
                        _emailSrv.NewArtistArtAgntRelationShip(ArtistAgent.Email, Artist.UserName, false, confirmUrl, removeUrl);
                    }
                    else
                    {
                        confirmUrl = baseUrl + "/Artist/PersonalManagment";
                        removeUrl = confirmUrl;
                        Artist.Artist.RelConfByArtistAgent = true;
                        _emailSrv.NewArtistArtAgntRelationShip(Artist.Email, ArtistAgent.UserName, true,confirmUrl,removeUrl);
                    }

                    _rep.Update(Artist);
                    _uow.Commit();

                    res.Success = true;
                }
                else
                {
                    res.Message = "לא הצלחנו למצוא את האמן או את הסוכן";
                }
            }
            catch (Exception e)
            {
                res.Message = e.Message;
                res.Success = false;
            }

            return res;
        }

        private void _consistArtistAgentRel(Artist art)
        {
            if (!art.RelConfByArtist && !art.RelConfByArtistAgent)
            {
                art.ArtistAgent = null;
            }
        }
    }
}
