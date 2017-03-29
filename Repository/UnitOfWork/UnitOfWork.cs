using Infrastructure.Domain;
using Model;
using Model.ContextDB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public DbContext context { get { return DataContextFactory.GetDataContext(); } }

        public UnitOfWork()
        {
        }
        public void RegisterAmended(IAggregateRoot entity, IUnitOfWorkRepository unitofWorkRepository)
        {
            unitofWorkRepository.PersistUpdateOf(entity);
        }

        public void RegisterNew(IAggregateRoot entity, IUnitOfWorkRepository unitofWorkRepository)
        {
            unitofWorkRepository.PersistCreationOf(entity);
        }

        public void RegisterRemoved(IAggregateRoot entity, IUnitOfWorkRepository unitofWorkRepository)
        {
            unitofWorkRepository.PersistDeletionOf(entity);
        }
        public void RegisterNewRange(IEnumerable<IAggregateRoot> entities, IUnitOfWorkRepository unitofWorkRepository)
        {
            unitofWorkRepository.PersistCreationRangeOf(entities);
        }

        public void Commit()
        {
            context.SaveChanges();
        }

    }
}
