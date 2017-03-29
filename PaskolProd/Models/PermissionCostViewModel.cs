using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaskolProd.Models
{
    public class PermissionCostViewModel
    {
        public List<PermissionsCategory> AllCategories { get; set; }
        public PermissionsCategory CurrCategory { get; set; }
        public bool? Success { get; set; }
    }
}