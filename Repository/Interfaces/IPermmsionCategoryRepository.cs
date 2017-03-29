﻿using Model;
using Model.PriceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IPermmsionCategoryRepository : IGenericRepository<PermissionsCategory, Guid>
    {
        IEnumerable<PermissionsCategory> GetAll(PermissionNavigationOptions options);
        PermissionsCategory GetByID(Guid id, PermissionNavigationOptions options);
    }
}
