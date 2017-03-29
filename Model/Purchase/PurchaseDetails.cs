using Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Purchase
{
    public class PurchaseDetails
    {
        public PaskolUser Customer { get; set; }
        public Music Music { get; set; }
        public double Cost { get; set; }
        public IEnumerable<string> PermissionValueNames { get; set; }
        public Permission Permission { get; set; }
        public bool IsExistPermission { get; set; }
    }
}
