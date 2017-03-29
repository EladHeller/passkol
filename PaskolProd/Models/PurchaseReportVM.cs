using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaskolProd.Models
{
    public class PurchaseReportVM
    {
        public string ID { get; set; }
        public string ArtistID { get; set; }
        public string AgreementFileID { get; set; }
        public Model.File Musicfile { get; set; }
        public double? Price { get; set; }
        public string DatePurchased { get; set; }
        public string MusicName { get; set; }
        public string ArtistName { get; set; }
        public string PermissionToString { get; set; }
    }
}