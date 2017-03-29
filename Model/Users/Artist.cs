using Infrastructure.Domain;
using Model.Auctions;
using Model.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Users
{
    public class Artist : IAggregateRoot
    {
        [Key,ForeignKey("User")]
        public string Id { get; set; }
        public string ContactManName { get; set; }
        public string ContactManPhone { get; set; }
        public string AgentName { get; set; }
        public string AgentPhone { get; set; }
        public string AgentMail { get; set; }
        public bool AgentVerified { get; set; }
        public string Biography { get; set; }
        public bool ParticipateInAuction { get; set; }
        public string PageUrl { get; set; }
        public PriceLevel PriceLevel { get; set; }

        public string  PhotoID { get; set; }
        public virtual PaskolUser User { get; set; }

        public virtual ArtistAgent ArtistAgent { get; set; }
        public bool RelConfByArtistAgent { get; set; }
        public bool RelConfByArtist { get; set; }

        public virtual TempArtist TempArtist { get; set; }

        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }

        public virtual ICollection<Auction> Auctions { get; set; }
        public virtual ICollection<Music> ArtistMusics { get; set; }
        //public string AuctionNotes { get; set; }
    }
}
