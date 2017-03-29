using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Users
{
    public enum UserType
    {
        [Display(Name = "אמן")]
        Artist,
        [Display(Name = "רוכש תוכן")]
        Customer,
        [Display(Name = "מנהל אמנים")]
        ArtistAgent,
        [Display(Name = "מנהל")]
        Admin
    }
}
