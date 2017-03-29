using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaskolProd.Models
{
    public class PermissionPropertyViewModel : PermissionProperty
    {
        public PermissionPropertyViewModel()
        {

        }
        public List<PermissionPropertyValueViewModel> PropertyValueViews { get; set; }

        public PermissionPropertyViewModel(PermissionProperty prop) : 
            base(prop)
        {
            this.PropertyValueViews = prop.PropertyValues
                .Where(val=> val.IsActive)
                    .Select(val=> new PermissionPropertyValueViewModel(val)).ToList();
        }
    }
}