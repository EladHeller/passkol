using Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaskolProd.Models
{
    public class UserDocumentsViewModel
    {
        public PaskolUser User { get; set; }
        public Dictionary<string, string> Documents { get; set; }
    }
}