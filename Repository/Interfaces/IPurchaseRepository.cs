using Model.Purchase;
using Repository.Messaging;
using System;
using System.Collections.Generic;
using Model.Logs;

namespace Repository.Interfaces
{
    public interface IPurchaseRepository : IGenericRepository<Purchase, string>
    {
        IEnumerable<Purchase> GetByCustomerId(string Id);
        IEnumerable<Purchase> GetByArtistId(string Id);
        IEnumerable<Purchase> GetByArtistAgntId(string[] ArtistId);
        RepositoryPagingResponse<Purchase> Get(int max, int skip, DateTime s, DateTime e, PurchaseStatus? status,bool WithoutPhonePurchase);
        IEnumerable<Purchase> GetByMusicId(int musicId);
        IEnumerable<Purchase> GetByDates(DateTime s, DateTime e);
        int GetWaitingForPaymentRegulatingCount();
        int GetMusicsPurchaseCount(DateTime? start, DateTime? end);
        RepositoryPagingResponse<LeadMusicLog> GetMusicsPurchase(DateTime? start, DateTime? end, int v, int resultInPage);
    }
}
