using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PasskolProd.Models
{
    public class PermissionViewModel
    {
        public PermissionViewModel()
        {

        }
        public PermissionViewModel(Permission perm)
        {
            this.ID = perm.ID;
            PermissionCategoryID = perm.PermissionCategoryID;
            PermissionCost = perm.PermissionCost;
            PropertyValuesIds = perm.PropertyValues.Select(val => val.ID);
        }
        public Guid ID { get; set; }
        public Guid PermissionCategoryID { get; set; }
        public double? PermissionCost { get; set; }
        public IEnumerable<Guid> PropertyValuesIds { get; set; }
    }
}