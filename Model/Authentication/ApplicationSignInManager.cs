using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Model.Authentication
{
    public class ApplicationSignInManager : SignInManager<PaskolUser, string>
    {
        public ApplicationSignInManager(PaskolUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(PaskolUser user)
        {
            return user.GenerateUserIdentityAsync((PaskolUserManager)UserManager);
        }

        public static ApplicationSignInManager Create
            (IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<PaskolUserManager>(), context.Authentication);
        }
    }
}
