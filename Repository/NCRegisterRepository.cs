using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System;
using System.Data.Entity.Infrastructure;
using Repository.UnitOfWork;
using Repository.Interfaces;
using Model;
using Model.Auctions;
using Repository.Messaging;
using Model.Logs;

namespace Repository
{
    public class NCRegisterRepository : GenericRepository<NCRegister, string> , INCRegisterRepository
    {
        public NCRegisterRepository(IUnitOfWork uof) : base(uof)
        {
        }

        public int GetNCRegisterCount(DateTime start, DateTime end, NCUserType type)
        {
            return dbSet.Where(nc => nc.NCUserType == type && nc.DateTime > start && nc.DateTime < end).Count();
        }

        public RepositoryPagingResponse<NCRegister> Get(int max, int skip, DateTime s, DateTime e, NCUserType? nCUserType)
        {
            RepositoryPagingResponse<NCRegister> res =
                new RepositoryPagingResponse<NCRegister>();

            IQueryable<NCRegister> query = this.dbSet.Include("User");

            if (s != null && e != null)
            {
                query = query.Where(p => p.DateTime >= s && p.DateTime <= e);
            }

            if (nCUserType != null)
            {
                query = query.Where(p => p.NCUserType == nCUserType);
            }

            res.TotalResults = query.Count();

            query = query.OrderBy(p => p.DateTime);
            res.Entities = query
                .Skip(skip)
                .Take(max)
                .ToList() as IEnumerable<NCRegister>;

            return res;
        }
    }
}
