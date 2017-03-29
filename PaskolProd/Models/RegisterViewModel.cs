using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PaskolProd.Models
{
    public class UserRegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "כתובת מייל")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, 
            ErrorMessage = "סיסמה חייבת להכיל לפחות 6 תווים, מתוכם לפחות מספר אחד ואות לועזית אחת",
            MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "סיסמא")]
        public string Password { get; set; }

        public string CaptchaId { get; set; }
        public string CaptchaInput { get; set; }
        public string InstanceId { get; set; }
    }

    public class ArtistRegisterViewModel : UserRegisterViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "שם אמן/להקה")]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "שם איש קשר")]
        public string ContactManName { get; set; }
        
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "טלפון איש קשר")]
        public string ContactManPhone { get; set; }
    }
    public class CustomerRegisterViewModel : UserRegisterViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "שם מלא")]
        public string Name { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "טלפון")]
        public string Phone { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "שם חברה")]
        public string CompanyName { get; set; }
    }

    public class MGRCustomerRegisterViewModel : CustomerRegisterViewModel
    {
        [DataType(DataType.Text)]
        [Display(Name = "הערה")]
        public string MgrNote{ get; set; }
    }

    public class MGRArtistRegisterViewModel : ArtistRegisterViewModel
    {
        [DataType(DataType.Text)]
        [Display(Name = "הערה")]
        public string MgrNote { get; set; }

        // Pic
        [DataType(DataType.Text)]
        [Display(Name = "כתובת עמוד אישי")]
        public string PageRoutName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "שם סוכן")]
        public string AgentName { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "טלפון סוכן")]
        public string AgentPhone { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "אימייל סוכן")]
        public string AgentMail { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "ביוגרפיה")]
        public string Biography { get; set; }

        [Display(Name = "מעוניין ליצור מוזיקה מקורית")]
        public bool ParticipateInAuction { get; set; }
    }

    public class MGRArtistAgentRegisterViewModel : MGRCustomerRegisterViewModel
    {
    }
}