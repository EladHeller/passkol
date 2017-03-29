using Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Repository.Messaging;
using Repository.UnitOfWork;
using Model.Confirm;
using Model.Purchase;
using System;
using System.Data.Entity;
using Model.Logs;

namespace Repository
{
    public class PurchaseRepository : GenericRepository<Purchase,string>, IPurchaseRepository
    {
        private new IQueryable<Purchase> dbSet
        {
            get
            {
                return base.dbSet.Where(p => p.NCPurchase == null);
            }
        }

        private IQueryable<Purchase> ConfirmedPurchase
        {
            get
            {
                return base.dbSet.Where(p => p.PurchaseCost > 0);
            }
        }

        public PurchaseRepository(IUnitOfWork uow) : base(uow)
        {
        }

        public int GetWaitingForPaymentRegulatingCount()
        {
            return ConfirmedPurchase.Where(p => p.PurchaseStatus == PurchaseStatus.NotPaidToArtist).Count();
        }

        public IEnumerable<Purchase> GetByCustomerId(string Id)
        {
            return ConfirmedPurchase
                .Where(p => p.Customer.Id == Id)
                .ToList();
        }

        public IEnumerable<Purchase> GetByArtistId(string Id)
        {
            return ConfirmedPurchase
                .Where(p => p.Music.ArtistID == Id)
                .ToList();
        }

        public IEnumerable<Purchase> GetByMusicId(int musicId)
        {
            return ConfirmedPurchase
                .Include("Customer")
                .Include("Music")
                .Include("Permision.PropertyValues.PermissionProperty")
                .Where(p => p.MusicID == musicId).ToList();
        }

        public IEnumerable<Purchase> GetByDates(DateTime s, DateTime e)
        {
            return ConfirmedPurchase
                .Where(p => p.PurchaseDate >= s && p.PurchaseDate <= e)
                .ToList();
        }

        public RepositoryPagingResponse<Purchase> Get(int max, int skip, DateTime s, DateTime e, PurchaseStatus? ps, bool WithoutPhonePurchase)
        {
            RepositoryPagingResponse<Purchase> res =
                new RepositoryPagingResponse<Purchase>();

            IQueryable<Purchase> query = ConfirmedPurchase;

            // Set Where
            if (s != null && e != null)
            {
                query = query.Where(p => p.PurchaseDate >= s && p.PurchaseDate <= e);
            }

            if (ps != null)
            {
                query = query.Where(p => p.PurchaseStatus == ps.Value);
            }

            if (WithoutPhonePurchase)
            {
                query = query.Where(p => p.PurchaseStatus != PurchaseStatus.NotPaidBycustomer);
            }
            query = query.OrderBy(p => p.PurchaseDate);
            res.TotalResults = query.Count();
            res.Entities = query
                .Skip(skip)
                .Take(max)
                .ToList() as IEnumerable<Purchase>;

            return res;
        }

        public int GetMusicsPurchaseCount(DateTime? start, DateTime? end)
        {
            return GetMusicPurchaseQuery(start, end).Count();
        }

        private IQueryable<IGrouping<int,Purchase>> GetMusicPurchaseQuery(DateTime? start, DateTime? end)
        {
            IQueryable<Purchase> query = ConfirmedPurchase;

            // Set Where
            if (start != null && end != null)
            {
                query = query.Where(p => p.PurchaseDate >= start && p.PurchaseDate <= end);
            }
            // Without phone purcase
            query = query.Where(p => p.PurchaseStatus != PurchaseStatus.NotPaidBycustomer);
            return query.GroupBy(p => p.MusicID);
        }

        public RepositoryPagingResponse<LeadMusicLog> GetMusicsPurchase(DateTime? start, DateTime? end, int skip, int resultInPage)
        {
            var query = GetMusicPurchaseQuery(start, end);
            var res = new RepositoryPagingResponse<LeadMusicLog>();
            res.TotalResults = query.Count();
            res.Entities = query.
                OrderBy(gr=>gr.Count()).
                Skip(skip).Take(resultInPage).
                ToList().
                Select(ConvertPurchaseGroupToLeadMusicLog);
            return res;
        }

        private LeadMusicLog ConvertPurchaseGroupToLeadMusicLog(IGrouping<int, Purchase> group)
        {
            var log = new LeadMusicLog();
            var purchase = group.First();
            var music = purchase?.Music;
            log.ArtistEarn = group.Sum(p => p.ArtistEarn ?? 0);
            log.ArtistName = music?.Artist?.User?.UserName;
            log.AvgCost = group.Average(p => p.PurchaseCost ?? 0);
            log.Count = group.Count();
            log.HebrewName = music?.HebrewName;
            log.MusicID = purchase != null ? purchase.MusicID : 0;
            log.PasskolEarn = group.Sum(p => p.PasskolEarn ?? 0);
            log.SumCosts = group.Sum(p => p.PurchaseCost ?? 0);

            return log;
        }

        public IEnumerable<Purchase> GetByArtistAgntId(string[] ArtistId)
        {
            return ConfirmedPurchase
                .Where(p => ArtistId.Any(id => id == p.Music.ArtistID))
                .ToList();
        }
    }
}
