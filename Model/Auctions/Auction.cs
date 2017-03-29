using Infrastructure.Domain;
using Model.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Auctions
{
    public class Auction : IAggregateRoot
    {
        public string AuctionId { get; set; }

        public string AuctionName { get; set; }
        public virtual PaskolUser Customer { get; set; }
        public virtual PaskolUser WinnerArtist { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? OpenDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? CloseDate { get; set; }
        //[DataType(DataType.DateTime)]
        //public DateTime? CloseAuctoinDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? OpenProjectDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? CloseProjectDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? PickWinnerDate { get; set; }
        public virtual ICollection<Sketch> Sketches { get; set; }
        public virtual ICollection<Artist> Participants { get; set; }
        public AuctionStatus AuctionStatus { get; set; }
        public string AuctionNote { get; set; }
        public string UsingName { get; set; }
        public string MusicProperties { get; set; }
        public int MusicLength { get; set; }
        [DataType(DataType.Currency)]
        public double AuctionBudget { get; set; }
        public string ContactName { get; set; }

        [Phone]
        [DataType(DataType.PhoneNumber)]
        public string ContactPhone { get; set; }

        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string ContactEmail { get; set; }
        public string Notes { get; set; }

        public Auction()
        {
            this.AuctionId = Guid.NewGuid().ToString();
        }
    }
}
