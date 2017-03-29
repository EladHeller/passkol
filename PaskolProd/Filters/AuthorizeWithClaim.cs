using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using PaskolProd.Authentication;

namespace Model.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AuthorizeWithClaim : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.User.IsImpersonating())
            {
                var originalRoles = HttpContext.Current.User.GetOriginalRoles().Split(',');
                if(originalRoles.Any(r => r == this.Roles))
                {
                    return true;
                }
            }
            return base.AuthorizeCore(httpContext);
        }
    }
}