using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Repository.UnitOfWork;
using System.Data.Entity;
using Model;

namespace Repository
{
    public class PermissionPropertyRepository : GenericRepository<PermissionProperty, Guid>, IPermissionPropertyRepository
    {
        public PermissionPropertyRepository(IUnitOfWork uof) : base(uof)
        {

        }

        public IEnumerable<PermissionProperty> GetAll(Guid categoryId)
        {
            return dbSet.Include("PermissionsCategory").Where(x => x.PermissionsCategoryID == categoryId).ToList();
        }

        public PermissionProperty GetByID(Guid id, bool includeValues)
        {
            IQueryable<PermissionProperty> query = this.dbSet;
            if (includeValues)
            {
                query = query.Include("PropertyValues");
            }

            return query.First(x=> x.ID == id);
        }
    }
}
