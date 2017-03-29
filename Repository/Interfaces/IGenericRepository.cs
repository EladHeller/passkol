using Repository.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IGenericRepository<T, TID>
    {
        IEnumerable<T> GetAll();
        T GetByID(TID id);
        void Add(T item);
        void AddRange(IEnumerable<T> items);
        void Update(T item);
        void Delete(T item);
        RepositoryPagingResponse<T> Get(int skip, int take);
    }
}
