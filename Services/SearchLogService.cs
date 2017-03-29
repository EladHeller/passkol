using Model.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Interfaces;
using Repository.UnitOfWork;

namespace Services
{
    public class SearchLogService : GenericServiceUOW<SearchLog, string>
    {
        public SearchLogService(IGenericRepository<SearchLog, string> repository, IUnitOfWork uow) : base(repository, uow)
        {
        }
    }
}
