using Model.Auctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaskolProd.Models
{
    public class AuctionListArtistViewModel : AuctionListViewModel
    {
        public bool? SketchSent { get; set; }
        public bool? Won { get; set; }
    }
}