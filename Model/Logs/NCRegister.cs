using Infrastructure.Domain;
using Model.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Logs
{
    public enum NCUserType
    {
        ArtistProfile,
        Register
    }

    public class NCRegister : IAggregateRoot
    {
        [Key, ForeignKey("User")]
        public string Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Note { get; set; }
        public NCUserType NCUserType { get; set; }
        public virtual PaskolUser User { get; set; }
        public NCRegister()
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }
}
