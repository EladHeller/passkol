using System;
using System.Collections.Generic;
using Model;

namespace Repository.Interfaces
{
    public interface IPermissionPropertyRepository : IGenericRepository<PermissionProperty, Guid>
    {
        IEnumerable<PermissionProperty> GetAll(Guid categoryId);
        PermissionProperty GetByID(Guid id, bool includeValues);
    }
}