using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DashboardData
    {
        public int RequestsForConfirmCount { get; set; }
        public int AuctionForConfirmCount { get; set; }
        public int AuctionForCloseCount { get; set; }
        public int PurchaseForPaymentRegulation { get; set; }
        public int NCRegisterCount { get; set; }
        public int NCProfileRegisterCount { get; set; }
        public int NCDataUploadCount { get; set; }
        public int NCPurchaseCount { get; set; }
    }
}
