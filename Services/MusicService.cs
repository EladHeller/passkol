using Infrastructure.Configuration;
using Model;
using Model.Auctions;
using Model.Users;
using Repository;
using Repository.Interfaces;
using Repository.Messaging;
using Repository.UnitOfWork;
using Services.Messaging.Requests;
using Services.Messaging.Responses;
using Services.PDFService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Services
{
    public class MusicService : GenericServiceUOW<Music, int>
    {
        private FileSystemService FsService { get; set; }
        private TagService TagService { get; set; }
        private IUsersRepository _usersRep { get; set; }
        private AuctionService _auctionSrv { get; set; }
        private PDFServiceSoapClient _pdfService;

        public MusicService(IMusicsRepository rep, IUnitOfWork uof, 
            FileSystemService fsService, TagService tagService, IUsersRepository userRep, AuctionService auctionSrv) : base(rep, uof)
        {
            FsService = fsService;
            TagService = tagService;
            _usersRep = userRep;
            _auctionSrv = auctionSrv;
            _pdfService = ServiceLocator.GetService<PDFServiceSoapClient>();
        }

        public EntityAllResponse<string> GetMusicNames(string searchMusicText, int max)
        {
            EntityAllResponse<string> res = new EntityAllResponse<string>();
            res.Success = false;

            try
            {
                res.Entities = (this._rep as MusicsRepository).GetMusicNames(searchMusicText, max);
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Message = e.ToString();
            }

            return res;
        }

        public ResponseBase AddOrRemoveFavouriteMusic(int musicId, PaskolUser user)
        {
            var res = new ResponseBase();
            try
            {
                var music = this._rep.GetByID(musicId);

                music.UsersFavouriteMusic = music.UsersFavouriteMusic ?? new List<PaskolUser>();
                var existUser = music.UsersFavouriteMusic.FirstOrDefault(x => x.Id == user.Id);
                if (existUser != null)
                {
                    music.UsersFavouriteMusic.Remove(existUser);
                }
                else
                {
                    music.UsersFavouriteMusic.Add(user);
                }
                this._rep.Update(music);
                this._uow.Commit();
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Success = false;
                res.Message = e.ToString();
            }

            return res;
        }

        public PagingResponse<Music> GetByArtistId(string artistId, SearchSortType sortType, PagingRequest pageRequest)
        {
            var res = new PagingResponse<Music>();

            try
            {
                RepositoryPagingResponse<Music> repositoryRes = (this._rep as IMusicsRepository).
                                            GetByArtistId(artistId,
                                                        pageRequest.ResultInPage,
                                                        (pageRequest.Page - 1) * pageRequest.ResultInPage,
                                                        sortType);
                res.Entities = repositoryRes.Entities;
                res.CurrPage = pageRequest.Page;
                res.ResultsInPage = pageRequest.ResultInPage;
                res.TotalResults = repositoryRes.TotalResults;

                res.Success = true;
            }
            catch (Exception e)
            {
                res.Success = false;
                res.Message = "אירעה שגיאה בחיפוש היצירות";
            }

            return res;
        }

        public PagingResponse<Music> GetBySearch(string searchText, IEnumerable<Guid> tagIds, PagingRequest pageRequest, SearchSortType sortType)
        {
            var res = new PagingResponse<Music>();

            try
            {
                RepositoryPagingResponse<Music> repositoryRes = (this._rep as IMusicsRepository).
                                            GetBySearch(searchText,
                                                        tagIds,
                                                        pageRequest.ResultInPage,
                                                        (pageRequest.Page - 1) * pageRequest.ResultInPage,
                                                        sortType);
                res.Entities = repositoryRes.Entities;
                res.CurrPage = pageRequest.Page;
                res.ResultsInPage = pageRequest.ResultInPage;
                res.TotalResults = repositoryRes.TotalResults;
                
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Success = false;
                res.Message = "אירעה שגיאה בחיפוש היצירות";
            }

            return res;
        }

        public PagingResponse<Music> GetBySearch(
            string searchMusicText,
            string searchArtistText,
            PagingRequest pageRequest, 
            MusicFieldsSort sortField = MusicFieldsSort.UpdateDate,
            bool sortDesc = true)
        {
            PagingResponse<Music> res = new PagingResponse<Music>();
            
            try
            {
                var repositoryRes = (this._rep as IMusicsRepository).
                    GetBySearch(searchMusicText,
                    searchArtistText,
                    pageRequest.ResultInPage,
                    (pageRequest.Page - 1) * pageRequest.ResultInPage,
                    sortField,
                    sortDesc);

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

        public ResponseBase Update(Music musicData, int musicId, Model.File WAVFile, Model.File MP3File)
        {
            ResponseBase res = this.GetByID(musicId);
            
            if (res.Success)
            {
                var dbMusic = (res as EntityResponse<Music>).Entity;
                dbMusic.UpdateDate = DateTime.Now;
                dbMusic.BPM = musicData.BPM;
                dbMusic.Comment = musicData.Comment;
                dbMusic.Copyrights = musicData.Copyrights;
                dbMusic.CostLevel = musicData.CostLevel;
                dbMusic.EnglishName = musicData.EnglishName;
                dbMusic.Exceptions = musicData.Exceptions;
                dbMusic.HebrewName = musicData.HebrewName;
                dbMusic.MusicComposer = musicData.MusicComposer;
                dbMusic.MusicLength = musicData.MusicLength;
                dbMusic.MusicPerformer = musicData.MusicPerformer;
                dbMusic.MusicWriter = musicData.MusicWriter;
                dbMusic.SongText = musicData.SongText;
                dbMusic.Status = musicData.Status;
                musicData.Tags = musicData.Tags ?? new List<Tag>();

                var addTagList = musicData.Tags
                    .Where(tg => !dbMusic.Tags.Any(dbTg => dbTg.ID == tg.ID))
                    ?.Select(tg => tg.ID);
                
                if (addTagList!=null && addTagList.Any())
                {
                    res = TagService.GetTagList(addTagList);
                    if (res.Success)
                    {
                        dbMusic.Tags.AddRange((res as EntityAllResponse<Tag>).Entities);
                    }
                }

                if (res.Success)
                {
                    dbMusic.Tags.RemoveAll(tg => !musicData.Tags.Any(x => x.ID == tg.ID));

                    if (WAVFile != null)
                    {
                        res = FsService.StoreFile(WAVFile);
                        if (res.Success)
                        {

                            if (dbMusic.WAVFile != null)
                            {
                                dbMusic.WAVFile.version = (res as EntityResponse<Model.File>).Entity.version;
                            }
                            else
                            {
                                dbMusic.WAVFile = (res as EntityResponse<Model.File>).Entity;
                                dbMusic.WAVFileID = dbMusic.WAVFile.FileId;
                            }
                        }
                    }
                }

                if (res.Success && MP3File != null)
                {
                    res = FsService.StoreFile(MP3File);
                    if (res.Success)
                    {
                        if (dbMusic.MP3File != null)
                        {
                            dbMusic.MP3File.version = (res as EntityResponse<Model.File>).Entity.version;
                        }
                        else
                        {
                            dbMusic.MP3File = (res as EntityResponse<Model.File>).Entity;
                            dbMusic.MP3FileID = dbMusic.MP3File.FileId;
                        }
                    }
                }

                if (res.Success)
                {
                    res = base.Update(dbMusic);
                }
            }
            
            return res;
        }

        public ResponseBase Add(Music music, Model.File WAVFile, Model.File MP3File)
        {
            var res = new ResponseBase();
            music.CreateDate = DateTime.Now;
            res.Success = true;
            if (music.Tags?.FirstOrDefault() != null)
            {
                var tgRes=TagService.GetAll();
                res.Success = tgRes.Success;
                if(res.Success)
                {
                    music.Tags = tgRes.Entities
                        .Where(dbTag => music.Tags.Any(tag => tag.ID == dbTag.ID))
                        .ToList();
                }
                else
                {
                    res.Message = tgRes.Message;
                }
            }
            if (music.Artist?.User?.UserName != null)
            {
                music.Artist = _usersRep.GetArtistByName(music.Artist.User.UserName)?.Artist;
                if (music.Artist == null)
                {
                    res.Success = false;
                    res.Message = "לא נמצא אמן עם שם זה";
                }
            }
            if (res.Success && WAVFile != null)
            {
                WAVFile.UserId = music.Artist.Id;

                res = FsService.StoreFile(WAVFile);
                if (res.Success)
                {
                    music.WAVFile = (res as EntityResponse<Model.File>).Entity;
                    music.WAVFileID = music.WAVFile.FileId;
                }
            }

            if (res.Success && MP3File != null)
            {
                MP3File.UserId = music.Artist.Id;
                res = FsService.StoreFile(MP3File);
                if (res.Success)
                {
                    music.MP3File = (res as EntityResponse<Model.File>).Entity;
                    music.MP3FileID = music.MP3File.FileId;
                }
            }
            if (res.Success)
            {
                if (music.MusicLength == TimeSpan.Zero)
                {
                    res.Success = false;
                    res.Message = "יש להזין אורך יצירה";
                }
            }
            if (res.Success)
            {
                // update async artist report by updating his pdf file
                _pdfService.ArtistAddMusicAsync(DateTime.Now, music.HebrewName,
                    music.MusicComposer, music.MusicPerformer, music.Exceptions, music.MusicWriter,
                    music.Artist.Id, WebConf.FSBaseRoute);
                res = base.Add(music);
            }
            return res;
        }

        public ResponseBase AddSketchToAuction(Auction auction, Sketch sketch, Model.File WAVFile, Model.File MP3File)
        {
            var res = new ResponseBase();
            sketch.CreateDate = DateTime.Now;
            res.Success = true;
            
            if(sketch.Artist == null && sketch.ArtistID != null)
            {
                sketch.Artist = _usersRep.GetByID(sketch.ArtistID)?.Artist;
            }
            if (res.Success && WAVFile != null)
            {
                WAVFile.UserId = sketch.Artist.Id;

                res = FsService.StoreFile(WAVFile);
                if (res.Success)
                {
                    sketch.WAVFile = (res as EntityResponse<Model.File>).Entity;
                    sketch.WAVFileID = sketch.WAVFile.FileId;
                }
            }

            if (res.Success && MP3File != null)
            {
                MP3File.UserId = sketch.Artist.Id;
                res = FsService.StoreFile(MP3File);
                if (res.Success)
                {
                    sketch.MP3File = (res as EntityResponse<Model.File>).Entity;
                    sketch.MP3FileID = sketch.MP3File.FileId;
                }
            }
            
            if (res.Success)
            {
                if(auction.Sketches == null)
                {
                    auction.Sketches = new List<Sketch>();
                }

                auction.Sketches.Add(sketch);
                return _auctionSrv.Update(auction);
            }
            return res;
        }

        public override ResponseBase ValidateItem(Music music)
        {
            ResponseBase res = new ResponseBase();
            res.Success = false;
            res.Message = String.Empty;
            if (music.ArtistID == null && music.Artist?.Id == null)
            {
                res.Message += "יש לקשר אמן ליצירה.\n";
            }
            if (music.Copyrights == null || music.Copyrights.Sum(copy =>copy.Percents) != 100)
            {
                res.Message += "סך זכויות היוצרים אינן 100%.\n";
            }
            if (string.IsNullOrWhiteSpace(music.HebrewName))
            {
                res.Message += "יש להזין שם בעברית ואנגלית.\n";
            }

            if (string.IsNullOrWhiteSpace(music.MusicComposer) ||
                string.IsNullOrWhiteSpace(music.MusicPerformer) ||
                string.IsNullOrWhiteSpace(music.MusicWriter))
            {
                res.Message += "יש להזין שם כותב, מלחין ומבצע.\n";
            }
            if (string.IsNullOrEmpty(music.MP3FileID))
            {
                res.Message += "יש לצרף קובץ MP3.\n";
            }
            if (string.IsNullOrEmpty(music.WAVFileID))
            {
                res.Message += "יש לצרף קובץ WAV.\n";
            }
            if (music.MusicLength == TimeSpan.FromSeconds(0))
            {
                res.Message += "יש להזין אורך מוזיקה\n";
            }
            if (res.Message == string.Empty)
            {
                res.Success = true;
            }
            return res;
        }

        public EntityResponse<int> GetMusicCount(DateTime? start, DateTime? end)
        {
            var res = new EntityResponse<int>();
            try
            {
                res.Entity = (_rep as IMusicsRepository).GetCount(start, end);
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Message = e.ToString();
                res.Success = false;
            }
            return res;
        }
    }
}
