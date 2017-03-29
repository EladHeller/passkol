using Model.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.UnitOfWork;

namespace Repository
{
    public class SearchLogRepository : GenericRepository<SearchLog, string>, ISearchLogRepository
    {
        public SearchLogRepository(IUnitOfWork uow) : base(uow)
        {
        }
    }
}
