using System.Collections.Generic;
using System.Linq;
using Model;
using System.Linq.Expressions;
using System;
using System.Data.Entity.Infrastructure;
using Repository.Messaging;
using Repository.UnitOfWork;
using Repository.Interfaces;
using System.Data.Entity;

namespace Repository
{
    public class MusicsRepository : GenericRepository<Music, int>, IMusicsRepository
    {
        new internal IQueryable<Music> dbSet
        {
            get
            {
                return base.dbSet.Where(m => m.NCDataUpload == null);
            }
        }

        internal IQueryable<Music> ActiveMusics
        {
            get
            {
                return dbSet.Where(msc => msc.Status == MusicActiveStatus.Edited || msc.Status == MusicActiveStatus.Public);
            }
        }

        public MusicsRepository(IUnitOfWork uow) : base(uow) {}
        private IOrderedQueryable<Music> OrderMusic(IQueryable<Music> query, MusicFieldsSort sortField, bool sortSesc)
        {
            IOrderedQueryable<Music> resQuery = null;
            switch (sortField)
            {
                case MusicFieldsSort.ArtistName:
                    if (sortSesc)
                    {
                        resQuery = query.OrderByDescending(x => x.Artist.User.UserName);
                    }
                    else
                    {
                        resQuery = query.OrderBy(x => x.Artist.User.UserName);
                    }
                    break;
                case MusicFieldsSort.MusicName:
                    if (sortSesc)
                    {
                        resQuery = query.OrderByDescending(x => x.HebrewName);
                    }
                    else
                    {
                        resQuery = query.OrderBy(x => x.HebrewName);
                    }
                    break;
                case MusicFieldsSort.CreateDate:
                    if (sortSesc)
                    {
                        resQuery = query.OrderByDescending(x => x.CreateDate);
                    }
                    else
                    {
                        resQuery = query.OrderBy(x => x.CreateDate);
                    }
                    break;
                case MusicFieldsSort.UpdateDate:
                    if (sortSesc)
                    {
                        resQuery = query.OrderByDescending(x => x.UpdateDate).ThenByDescending(x => x.CreateDate);
                    }
                    else
                    {
                        resQuery = query.OrderBy(x => x.UpdateDate);
                    }
                    break;
                case MusicFieldsSort.None:
                        resQuery = query.OrderBy(x => x.ID);
                    break;
            }
            return resQuery;
        }

        public RepositoryPagingResponse<Music> GetBySearch(
            string searchMusicText, 
            string searchArtistText,
            int max, 
            int skip, 
            MusicFieldsSort sortField, 
            bool orderDesc)
        {
            var res = new RepositoryPagingResponse<Music>();
            searchArtistText = searchArtistText ?? string.Empty;
            searchMusicText= searchMusicText ?? string.Empty;
            Expression<Func<Music, bool>> predicate = (msc => msc.HebrewName.StartsWith(searchMusicText) &&
                                        msc.Artist.User.UserName.StartsWith(searchArtistText));
            IQueryable<Music> query = this.dbSet.Include("Artist.User").Where(predicate);
            res.TotalResults = query.Count(); 
            IOrderedQueryable< Music > orDquery = OrderMusic(query, sortField, orderDesc);
            
            res.Entities = orDquery.Skip(skip).Take(max).ToList();
            return res;
        }

        public IEnumerable<string> GetMusicNames(
            string searchMusicText,
            int max)
        {
            searchMusicText = searchMusicText ?? string.Empty;
            Expression<Func<Music, bool>> predicate = (msc => msc.HebrewName.StartsWith(searchMusicText));
            IQueryable<Music> query = this.dbSet.Where(predicate);
            IOrderedQueryable<Music> ordQuery = OrderMusic(query, MusicFieldsSort.None, false);
            return ordQuery.Take(max).Select(x => x.HebrewName).ToList();
        }

        public override IEnumerable<Music> GetAll()
        {
            return this.dbSet.Include("Artist.User").ToList();
        }

        public override Music GetByID(int id)
        {
            return this.dbSet.Include("Artist.User").Include("Tags.ParentTag").Include("Copyrights").First(msc => msc.ID == id);   
        }

        public int GetCount(DateTime? start, DateTime? end)
        {
            IQueryable<Music> query = dbSet.Where(msc => msc.Status != MusicActiveStatus.WaitingForConfirm);
            if (start.HasValue && end.HasValue && end.Value > DateTime.MinValue)
            {
                query = query.Where(music => music.CreateDate > start.Value && music.CreateDate < end.Value);
            }
            return query.Count();
        }

        public RepositoryPagingResponse<Music> GetBySearch(string searchText, IEnumerable<Guid> tagIds, 
            int max, int skip,SearchSortType sortType)
        {
            var res = new RepositoryPagingResponse<Music>();
            IQueryable<Music> query = ActiveMusics;

            if (searchText.Trim() != string.Empty)
            {
                query = query.Where(msc => msc.HebrewName.Contains(searchText) || msc.SongText.Contains(searchText));
            }

            if (tagIds.Any())
            {
                foreach (Guid tgId in tagIds)
                {
                    query = query.Where(msc => msc.Tags.Any(tag=> tag.ID == tgId));
                }
            }
            res.TotalResults = query.Count();
            IOrderedQueryable<Music> orderQuery = null;

            switch (sortType)
            {
                case SearchSortType.Relevant:
                    orderQuery = query.OrderByDescending(msc => (msc.HebrewName == searchText) ? 4 : (msc.HebrewName.Contains(searchText) ? 2 : 1));
                    break;
                case SearchSortType.LengthDesc:
                    orderQuery = query.OrderByDescending(msc => msc.MusicLength);
                    break;
                case SearchSortType.LengthAsc:
                    orderQuery = query.OrderBy(msc => msc.MusicLength);
                    break;
            }
            res.Entities = orderQuery
                    .Skip(skip)
                    .Take(max)
                    .ToList();
            return res;
        }

        public RepositoryPagingResponse<Music> GetByArtistId(string artistId, int max, int skip, SearchSortType sortType)
        {
            var res = new RepositoryPagingResponse<Music>();
            var query = ActiveMusics.Where(m => m.ArtistID == artistId);
            res.TotalResults = query.Count();
            IOrderedQueryable<Music> orderQuery = null;

            switch (sortType)
            {
                case SearchSortType.LengthDesc:
                    orderQuery = query.OrderByDescending(msc => msc.MusicLength);
                    break;
                case SearchSortType.LengthAsc:
                    orderQuery = query.OrderBy(msc => msc.MusicLength);
                    break;
                default:
                    orderQuery = query.OrderByDescending(msc => msc.CreateDate);
                    break;
            }
            res.Entities = orderQuery
                    .Skip(skip)
                    .Take(max)
                    .ToList();
            return res;
        }
    }
}
