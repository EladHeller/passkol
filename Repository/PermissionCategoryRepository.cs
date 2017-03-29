using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Repository.UnitOfWork;
using System.Data.Entity;
using Model.PriceModel;
using Model;

namespace Repository
{
    public class PermissionCategoryRepository : GenericRepository<PermissionsCategory, Guid>, IPermmsionCategoryRepository
    {
        public PermissionCategoryRepository(IUnitOfWork uof) : base(uof)
        { }

        public PermissionsCategory GetByID(Guid id, PermissionNavigationOptions options)
        {
            return GetQuery(options).First(perCat => perCat.ID == id);
        }

        public IEnumerable<PermissionsCategory> GetAll(PermissionNavigationOptions options)
        {
            return GetQuery(options).ToList();
        }

        private IQueryable<PermissionsCategory> GetQuery(PermissionNavigationOptions options)
        {
            IQueryable<PermissionsCategory> query = this.dbSet;
            switch (options)
            {
                case PermissionNavigationOptions.None:
                    break;
                case PermissionNavigationOptions.IncludeProperties:
                    query = query.Include("PermissionProperties");
                    break;
                case PermissionNavigationOptions.IncludeAll:
                    query = query.Include("PermissionProperties.PropertyValues.PermissionPrices");
                    break;
            }

            return query;
        }
    }
}
