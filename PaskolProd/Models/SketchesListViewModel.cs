using Model.Auctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaskolProd.Models
{
    public class SketchesListViewModel
    {
        public string ArtistId { get; set; }
        public Model.File File { get; set; }
        public string EndTime { get; set; }
        public string PickWinnerTime { get; set; }
        public string AuctionId { get; set; }
        public string SketchID { get; set; }
        public string SketchesName { get; set; }
        public string Note { get; set; }
        public string ArtistName { get; set; }
    }
}