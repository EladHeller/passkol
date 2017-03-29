using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PaskolProd;
using Model.Users;
using Model.Helper;
using Model;

namespace PaskolProd.Models
{
    public class ManagersCreateView : ManagersEditView
    {
        public ManagersCreateView()
        {
        }
        public ManagersCreateView(PaskolUser paskolUser) : base(paskolUser)
        {
        }

        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "סיסמה")]
        public override string Password { get; set; }
    }

    public class ManagersEditView : UserEditView
    {
        public ManagersEditView()
        {
        }

        public ManagersEditView(PaskolUser user) : base(user)
        {
            Roles = user.Roles.Select(r => r.RoleId).ToList();
        }

        [Display(Name = "הרשאות")]
        public IEnumerable<string> Roles { get; set; }
    }

    public class UserEditView
    {
        public UserEditView() {}

        public UserEditView(PaskolUser user)
        {
            UserID = user.Id;
            Name = user.UserName;
            Email = user.Email;
            Role = user.UserType.ToDescription();
            RegisteredDate = user.RegisteredDate;
            Status = user.Status.ToString();
            Phone = user.PhoneNumber;
            // סיסמה תוחלף רק את יוזן ערך בשדה. הסיסמה הקיימת לא תוצג!
            Note = user.Note;
        }

        public string UserID { get; set; }

        [Required]
        [StringLength(25)]
        [DataType(DataType.Text)]
        [Display(Name = "שם")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "אמייל")]
        public string Email { get; set; }

        [Phone]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "טלפון")]
        public string Phone { get; set; }

        [Display(Name = "סוג")]
        public string Role { get; set; }

        [Display(Name = "תאריך רישום")]
        public DateTime RegisteredDate { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "הערת מנהל")]
        public string Note { get; set; }

        [Display(Name = "סטאטוס")]
        public string Status { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "סיסמה")]
        public virtual string Password { get; set; }
    }

    public class CustomerCreateView : CustomerEditView
    {
        public CustomerCreateView()
        {
        }
        public CustomerCreateView(PaskolUser paskolUser) : base(paskolUser)
        {
        }

        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "סיסמה")]
        public override string Password { get; set; }
    }

    public class CustomerEditView : UserEditView
    {
        public CustomerEditView()
        {
        }

        public CustomerEditView(PaskolUser user) : base(user)
        {
            CompanyName = user.Customer.CompanyName;
        }

        [DataType(DataType.Text)]
        [Display(Name = "שם חברה")]
        public string CompanyName { get; set; }
    }

    public class ArtistEditView : UserEditView
    {

        public ArtistEditView()
        {

        }

        public ArtistEditView(PaskolUser user) : base(user)
        {
            PictureId = user.Artist.PhotoID != null ? user.Artist.PhotoID : null;
            ContactManName = user.Artist.ContactManName;
            ContactManPhone = user.Artist.ContactManPhone;
            PublicPage = user.Artist.PageUrl;
            AgentEmail = user.Artist.AgentMail;
            AgentName = user.Artist.AgentName;
            AgentPhone = user.Artist.AgentPhone;
            AgentVerified = user.Artist.RelConfByArtist;
            Biography = user.Artist.Biography;
            ParticipateInAuction = user.Artist.ParticipateInAuction;
            PriceLevel = user.Artist.PriceLevel;
        }

        [Display(Name = "תמונה")]
        public string PictureId { get; set; }

        [StringLength(15)]
        [DataType(DataType.Text)]
        [Display(Name = "איש קשר")]
        public string ContactManName { get; set; }

        [Phone]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "טלפון איש קשר")]
        public string ContactManPhone { get; set; }

        [Display(Name = "עמוד ציבורי")]
        public string PublicPage { get; set; }

        [StringLength(15)]
        [DataType(DataType.Text)]
        [Display(Name = "שם סוכן")]
        public string AgentName { get; set; }

        [Phone]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "טלפון סוכן")]
        public string AgentPhone { get; set; }

        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "אמייל סוכן")]
        public string AgentEmail { get; set; }

        [Display(Name = "סוכן אושר")]
        public bool AgentVerified { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "ביוגרפיה")]
        public string Biography { get; set; }

        [Display(Name = "משתתף במכרזים")]
        public bool ParticipateInAuction { get; set; }

        [Display(Name = "רמת מחיר")]
        public PriceLevel PriceLevel { get; set; }

    }

    public class ArtistCreateView : ArtistEditView
    {
        public ArtistCreateView()
        {
        }
        public ArtistCreateView(PaskolUser paskolUser) : base(paskolUser)
        {
        }

        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "סיסמה")]
        public override string Password { get; set; }
    }

    public class ArtistAgentEditView : UserEditView
    {
        public ArtistAgentEditView()
        {
        }

        public ArtistAgentEditView(PaskolUser user)
            : base(user)
        {
            this.Artists = user.ArtistAgent.Artists.Select(a => a.User.UserName);
            CompanyName = user.ArtistAgent.CompanyName;
        }

        [Display(Name = "אמנים")]
        public IEnumerable<string> Artists { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "שם חברה")]
        public string CompanyName { get; set; }
    }

    public class ArtistAgentCreateView : ArtistAgentEditView
    {
        public ArtistAgentCreateView()
        {
        }
        public ArtistAgentCreateView(PaskolUser paskolUser) : base(paskolUser)
        {
        }

        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "סיסמה")]
        public override string Password { get; set; }
    }
}