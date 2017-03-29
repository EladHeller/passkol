using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaskolProd.Models
{
    public class PermissionPropertyValueViewModel : PermissionPropertyValue
    {
        public PermissionPropertyValueViewModel()
        {

        }
        public PermissionPropertyValueViewModel(PermissionPropertyValue val) 
            : base(val)
        {
            IsDeleted = false;
        }

        public bool IsDeleted { get; set; }
    }
}