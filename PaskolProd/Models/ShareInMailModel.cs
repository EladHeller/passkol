using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PaskolProd.Models
{
    public class ShareInMailModel
    {
        [Required]
        [EmailAddress]
        public string UserMail { get; set; }
        [Required]
        [EmailAddress]
        public string FriendMail { get; set; }
        public string Comments { get; set; }
        public bool SendCopy { get; set; }
        [Required]
        public int MusicId { get; set; }
    }
}