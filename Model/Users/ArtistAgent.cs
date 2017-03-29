using Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Users
{
    public class ArtistAgent : IAggregateRoot
    {
        [Key, ForeignKey("User")]
        public string Id { get; set; }
        public string CompanyName { get; set; }
        public virtual ICollection<Artist> Artists { get; set; }
        public virtual PaskolUser User { get; set; }
    }
}
