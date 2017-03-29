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
    public interface INCRegisterRepository : IGenericRepository<NCRegister, string>
    {
        RepositoryPagingResponse<NCRegister> Get(int max, int skip, DateTime s, DateTime e, NCUserType? nCUserType);
        int GetNCRegisterCount(DateTime start, DateTime end, NCUserType type);
    }
}
