using Model.Auctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaskolProd.Models
{
    public class AuctionListViewModel
    {
        public string Id { get; set; }
        public string AuctionName { get; set; }
        public string OpenDate { get; set; }
        public string CloseDate { get; set; }
        public string OpenProjectDate { get; set; }
        public string PickWinnerDate { get; set; }
        public string AuctionStatus { get; set; }
    }
}