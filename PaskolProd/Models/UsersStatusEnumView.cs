using System.ComponentModel.DataAnnotations;

namespace PaskolProd.Models
{
    public enum UsersStatusEnumView
    {
        [Display(Name = "רשומים")]
        Registered,
        [Display(Name ="חדשים")]
        New,
        [Display(Name = "פעילים")]
        Active
    }
}