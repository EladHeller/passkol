using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using PaskolProd.Models;
using PaskolProd;
using Model;
using Model.Authentication;

namespace PaskolProd
{
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(PaskolDbContext.Create);
            app.CreatePerOwinContext<PaskolUserManager>(PaskolUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            app.CreatePerOwinContext<PaskolRoleManager>(PaskolRoleManager.Create);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                ExpireTimeSpan = TimeSpan.FromMinutes(30),
                LoginPath = PathString.FromUriComponent("/"),
                ReturnUrlParameter = "#lf"
            });
        }
    }
}