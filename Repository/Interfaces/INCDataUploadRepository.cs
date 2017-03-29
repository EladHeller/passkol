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
    public interface INCDataUploadRepository : IGenericRepository<NCDataUpload, int>
    {
        RepositoryPagingResponse<NCDataUpload> Get(int max, int skip, DateTime s, DateTime e);
        int GetNCDataUploadCountByDates(DateTime start, DateTime end);
    }
}
