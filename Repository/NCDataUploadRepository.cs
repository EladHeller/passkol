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
    public class NCDataUploadRepository : GenericRepository<NCDataUpload, int> , INCDataUploadRepository
    {
        public NCDataUploadRepository(IUnitOfWork uof) : base(uof)
        {
        }

        public int GetNCDataUploadCountByDates(DateTime start, DateTime end)
        {
            return dbSet.Where(nc => nc.DateTime > start && nc.DateTime < end).Count();
        }

        public RepositoryPagingResponse<NCDataUpload> Get(int max, int skip, DateTime s, DateTime e)
        {
            RepositoryPagingResponse<NCDataUpload> res =
                new RepositoryPagingResponse<NCDataUpload>();

            IQueryable<NCDataUpload> query = this.dbSet.Include("Music");

            if (s != null && e != null)
            {
                query = query.Where(p => p.DateTime >= s && p.DateTime <= e);
            }
            
            res.TotalResults = query.Count();

            query = query.OrderBy(p => p.DateTime);
            res.Entities = query
                .Skip(skip)
                .Take(max)
                .ToList() as IEnumerable<NCDataUpload>;

            return res;
        }
    }
}
