using System.ComponentModel.DataAnnotations;

namespace Model.Confirm
{
    public enum ConfirmType
    {
        [Display(Name = "הכל")]
        All,
        [Display(Name = "רכישה טלפונית")]
        PhonePurchase,
        [Display(Name = "אמן חדש")]
        NewArtist,
        [Display(Name = "עדכון אמן")]
        UpdateArtist,
        [Display(Name = "יצירה חדשה")]
        NewMusic,
        [Display(Name = "עדכון יצירה")]
        UpdateMusic
    }
}