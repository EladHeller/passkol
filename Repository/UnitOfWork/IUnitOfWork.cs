using Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.UnitOfWork
{
    public interface IUnitOfWork
    {
        DbContext context { get; }
        void RegisterAmended(IAggregateRoot entity,
            IUnitOfWorkRepository unitofWorkRepository);
        void RegisterNew(IAggregateRoot entity,
            IUnitOfWorkRepository unitofWorkRepository);
        void RegisterRemoved(IAggregateRoot entity,
            IUnitOfWorkRepository unitofWorkRepository);
        void Commit();
        void RegisterNewRange(IEnumerable<IAggregateRoot> entities, IUnitOfWorkRepository genericRepository);
    }
}
