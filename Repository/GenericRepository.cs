using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Domain;
using System.Data.Entity;
using Model;
using Repository.Messaging;
using Repository.Interfaces;
using Repository.UnitOfWork;
using System.Linq.Expressions;
using Model.ContextDB;

namespace Repository
{
    public class GenericRepository<T, TID> : IUnitOfWorkRepository where T : class,IAggregateRoot
    {
        internal virtual DbSet<T> dbSet
        {
            get
            {
                return DataContextFactory.GetDataContext().Set<T>();
            }
        }

        internal IUnitOfWork _uow;

        public GenericRepository(IUnitOfWork uow)
        {
            this._uow = uow;
        }

        public void Add(T entity)
        {
            _uow.RegisterNew(entity, this);
        }

        public virtual void AddRange(IEnumerable<T> entities)
        {
            _uow.RegisterNewRange(entities, this);
        }

        public virtual void Delete(T entity)
        {
            _uow.RegisterRemoved(entity, this);
        }

        public virtual void Update(T entity)
        {
            _uow.RegisterAmended(entity, this);
        }
        
        public virtual T GetByID(TID id)
        {
            return dbSet.Find(id);
        }
        
        public virtual IEnumerable<T> GetAll()
        {
            return this.dbSet.ToList();
        }

        public IEnumerable<T> FindAll(int index, int count)
        {
            return this.dbSet.Skip(index).Take(count).ToList<T>();
        }
        
        public virtual RepositoryPagingResponse<T> Get(int skip, int take)
        {
            RepositoryPagingResponse<T> res = new RepositoryPagingResponse<T>();
            var query = this.dbSet;
            res.TotalResults = query.Count();
            res.Entities = query.Take(take).Skip(skip);
            return res;
        }

        public void PersistCreationOf(IAggregateRoot entity)
        {
            dbSet.Add((T)entity);
        }

        public void PersistUpdateOf(IAggregateRoot entity)
        {
            // Do nothing as EF tracks changes
        }

        public void PersistDeletionOf(IAggregateRoot entity)
        {
            dbSet.Remove((T)entity);
        }

        public void PersistCreationRangeOf(IEnumerable<IAggregateRoot> entities)
        {
            dbSet.AddRange((IEnumerable<T>)entities);
        }
    }
}
