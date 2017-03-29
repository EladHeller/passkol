using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.UnitOfWork;
using Repository.Interfaces;

namespace Repository
{
    public class PermissionRepository : GenericRepository<Permission, Guid>, IPermmsionRepository
    {
        public PermissionRepository(IUnitOfWork uow) : base(uow)
        {
        }
    }
}
