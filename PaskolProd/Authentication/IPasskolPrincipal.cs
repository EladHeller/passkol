using Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace PaskolProd.Authentication
{
    interface IPasskolPrincipal : IPrincipal
    {
        UserType UserType { get; set; }
    }
}