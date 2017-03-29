using Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Repository.Messaging;
using Repository.UnitOfWork;
using Model.Confirm;

namespace Repository
{
    public class ConfirmationRepository : GenericRepository<Confirmation,int>, IConfirmationRepository
    {
        public ConfirmationRepository(IUnitOfWork uof) : base(uof)
        {
        }

        public Confirmation GetByEntityId(string Id)
        {
            return this.dbSet.SingleOrDefault(c => c.EntityId == Id);
        }

        public int GetConfirmationCount()
        {
            return this.dbSet.Count();
        }

        public RepositoryPagingResponse<Confirmation> GetBySearch(IList<ConfirmType> Types ,int max,int skip, ConfirmSorting sortOrder)
        {
            RepositoryPagingResponse<Confirmation> res = 
                new RepositoryPagingResponse<Confirmation>();

            IQueryable<Confirmation> query = this.dbSet;

            if (Types != null)
            {
                query = query.Where(con => Types.Any(t => t == con.ConfirmType));
            }
            
            res.TotalResults = query.Count();

            // Sort by param
            switch (sortOrder)
            {
                case ConfirmSorting.type_desc:
                    query = query.OrderByDescending(c => c.ConfirmType);
                    break;
                case ConfirmSorting.date:
                    query = query.OrderBy(c => c.DateUpdate);
                    break;
                case ConfirmSorting.date_desc:
                    query = query.OrderByDescending(c => c.DateUpdate);
                    break;
                default:
                    query = query.OrderBy(c => c.ConfirmType);
                    break;
            }
            
            res.Entities = query
                .Skip(skip)
                .Take(max)
                .ToList() as IEnumerable<Confirmation>;
            
            return res;
        }
    }
}
