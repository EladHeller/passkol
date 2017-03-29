using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaskolProd.Models
{
    public class AuctionRequirmentsViewModel
    {
        public string AuctionId { get; set; }
        public string Using { get; set; }
        public string Open { get; set; }
        public string End { get; set; }
        public double MusicLength { get; set; }
        public double Budget { get; set; }
        public string ContactMan { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Notes { get; set; }
        public string MusicProperties { get; set; }
    }
}