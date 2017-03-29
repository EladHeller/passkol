using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Authentication
{
    public enum PasskolRoles
    {
        [Display(Name = "מנהל מערכת ")]
        SystemAdmin,
        [Display(Name = "פרופיל - לעריכת פרופיל אמן")]
        AdminProfile,
        [Display(Name = "תיוג - לעריכת מאפייני זאנר גלובאליים")]
        AdminLabel,
        [Display(Name = "יצירות – לעריכת יצירות")]
        AdminMusicEditor,
        [Display(Name = "כספים – לצפייה ועדכון דוחות עם נתוני כספים")]
        AdminFinance,
        [Display(Name = "אמן")]
        Artist,
        [Display(Name = "רוכש")]
        Customer,
        [Display(Name = "מנהל אמנים")]
        ArtistAgent
    }
}
