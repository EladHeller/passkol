using Model;
using PaskolProd.Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PasskolProd.Models
{
    public class PermissionCategoryViewModel
    {
        private static PermissionComparer permissionComperer = new PermissionComparer();

        public PermissionCategoryViewModel(
            PermissionsCategory category, 
            bool getProperties = false, 
            bool getPermissions = false,
            bool includeCostProperties = false)
        {
            ID = category.ID;
            Name = category.Name;
            IsActive = category.IsActive;
            ViewOrder = category.ViewOrder;
            if (getProperties)
            {
                PermissionProperties = 
                    category.PermissionProperties
                        .Where(prop=> prop.IsActive && (includeCostProperties || !prop.IsCostLevel))
                            .Select(prop => new PermissionPropertyViewModel(prop));
            }

            if (getPermissions)
            {
                Permissions = category.PermissionProperties.Where(prop => prop.IsActive)
                        .SelectMany(prop => prop.PropertyValues.Where(val => val.IsActive)
                            .SelectMany(val => val.PermissionPrices))
                                .Distinct(permissionComperer)
                                    .Select(perm => new PermissionViewModel(perm));
            }
        }

        public Guid ID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int ViewOrder { get; set; }
        public IEnumerable<PermissionPropertyViewModel> PermissionProperties { get; set; }
        public IEnumerable<PermissionViewModel> Permissions { get; set; }
    }
}