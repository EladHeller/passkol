using Model.Logs;
using Repository.Interfaces;

namespace Repository
{
    interface ISearchLogRepository :IGenericRepository<SearchLog, string>
    {
    }
}