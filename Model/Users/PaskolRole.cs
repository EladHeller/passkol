using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.Users
{
    public class PaskolRole : IdentityRole
    {
        public PaskolRole() : base() { }
        public PaskolRole(string name) : base(name) { }
    }

}