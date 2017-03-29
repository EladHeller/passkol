using Model;
using Model.Auctions;
using Model.Logs;
using Repository.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface INCPurchaseRepository : IGenericRepository<NCPurchase, string>
    {
        RepositoryPagingResponse<NCPurchase> Get(int max, int skip, DateTime s, DateTime e);
        int GetNCPurchaseCountByDates(DateTime start, DateTime end);
    }
}
