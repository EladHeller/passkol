using Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Users
{
    public enum UserStatus 
    {
        [Display(Name = "פעיל")]
        Active,
        [Display(Name = "חסום")]
        Blocked,
        [Display(Name = "אמן חדש ממתין לאישור")]
        WaitingNewArtist,
        [Display(Name = "עדכון אמן ממתין לאישור")]
        WaitingEditedArtist
    }
}
