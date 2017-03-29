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
    public class TempArtist
    {
        [Key, ForeignKey("Artist")]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public virtual Artist Artist { get; set; }
    }
}
