using Model;
using Model.Auctions;
using Repository.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IAuctionRepository : IGenericRepository<Auction, string>
    {
        RepositoryPagingResponse<Auction> Get(int max, int skip, AuctionSort sortOrder, string UserId = null);
        int GetAuctionsCountByStatus(AuctionStatus status);
        IEnumerable<Auction> GetAuctionsForCustomer(string userID);
        IEnumerable<Auction> GetAuctionsForArtist(string userID);
    }
}
