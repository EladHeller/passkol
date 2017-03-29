using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Model.Users;

namespace PaskolProd.Authentication
{
    public class PasskolPrincipal : IPasskolPrincipal
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role)
        {
            return false;
        }

        public PasskolPrincipal(string email)
        {
            this.Identity = new GenericIdentity(email);
        }
        
        public UserType UserType { get; set; }
    }
}