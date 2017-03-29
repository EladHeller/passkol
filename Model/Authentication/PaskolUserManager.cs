using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Model.Users;

namespace Model.Authentication
{
    public class PaskolUserManager : UserManager<PaskolUser>
    {
        public PaskolUserManager(IUserStore<PaskolUser> store)
            : base(store)
        {
        }

        public static PaskolUserManager Create(IdentityFactoryOptions<PaskolUserManager> options, IOwinContext context)
        {
            var manager = new PaskolUserManager(new PaskolUserStore(context.Get<PaskolDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<PaskolUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireDigit = true,
                RequireLowercase = true
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;
            
            return manager;
        }
    }
}
