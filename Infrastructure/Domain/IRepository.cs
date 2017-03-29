using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Domain
{
    public interface IRepository<T, TId> : IReadOnlyRepository<T, TId> where T : IAggregateRoot
    {
        IQueryable<T> Table {get;}
        void Save(T entity);
        void Add(T entity);
        void Remove(T entity);
    }
}
