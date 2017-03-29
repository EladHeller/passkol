using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using Model.Users;
using Model.Authentication;
using Model.Helper;
using PaskolProd.Authentication;
using Services;
using System.Security.Claims;
using Model.Filters;
using System.Globalization;

namespace PaskolProd.Areas.Admin.Controllers
{
    public class UserBaseController : Controller
    {
        /// <summary>
        /// The User Manager Stored at the HttpContext
        /// </summary>
        internal PaskolUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<PaskolUserManager>();
            }
        }

        /// <summary>
        /// The Role Manager Stored at the HttpContext
        /// </summary>
        internal PaskolRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<PaskolRoleManager>();
            }
        }

        internal PaskolEmailService EmailService
        {
            get
            {
                return ServiceLocator.GetService<PaskolEmailService>();
            }
        }
        /// <summary>
        /// The Singin Manager Stored at the HttpContext
        /// </summary>
        internal ApplicationSignInManager SignInManager
        {
            get
            {
                return HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
        }

        internal IEnumerable<EnumKeyVal> GetAdminRoles()
        {
            return Enum.GetValues(typeof(PasskolRoles))
                        .Cast<PasskolRoles>()
                        .Where(t => t == PasskolRoles.AdminFinance || 
                                    t == PasskolRoles.AdminLabel || 
                                    t == PasskolRoles.AdminMusicEditor ||
                                    t == PasskolRoles.AdminProfile ||
                                    t == PasskolRoles.SystemAdmin)
                        .Select(e => new EnumKeyVal()
                        {
                            Key = e.ToString(),
                            Value = e.ToDescription()
                        })
                        .ToList();
        }

        [Authorize(Roles = "SystemAdmin,AdminProfile")]
        public JsonResult GetUserByName(string StartWith, UserType? usrType)
        {
            var query = this.UserManager.Users
                .Where(u => u.UserName.StartsWith(StartWith));
            if (usrType.HasValue)
            {
                query = query.Where(u => u.UserType == usrType);
            }

            return Json(query
                .Select(u => u.UserName)
                .ToList(), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "SystemAdmin,AdminProfile")]
        public JsonResult GetUserByContactManName(string StartWith)
        {
            return Json(this.UserManager.Users
                .Where(u => u.Artist != null &&
                            u.Artist.ContactManName.StartsWith(StartWith))
                .Select(u => u.Artist.ContactManName)
                .ToList(), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "SystemAdmin")]
        public JsonResult GetNewArtist(string name, IEnumerable<string> pickedArtists)
        {
            pickedArtists = pickedArtists ?? new List<string>();

            return Json(this.UserManager.Users
                .Where(u => u.Artist != null &&
                            u.Artist.ParticipateInAuction &&
                            u.UserName.Contains(name) &&

                            pickedArtists.All(p => p != u.UserName))
                .Select(u => u.UserName)
                .ToList(), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "SystemAdmin")]
        public JsonResult GetAllArtists()
        {
            return Json(this.UserManager.Users
                .Where(u => u.UserType == UserType.Artist)
                .Select(u => u.UserName)
                .ToList(), JsonRequestBehavior.AllowGet);
        }

        internal async Task<IdentityResult> CreateUserAsync
            (PaskolUser user, string Password, IEnumerable<string> roles)
        {
            IdentityResult result;

            try
            {
                // Try to create the user
                result = await UserManager.CreateAsync(user, Password);

                if (result.Succeeded)
                {
                    if (roles != null)
                    {
                        foreach (string role in roles)
                        {
                            // assign user artist to role
                            await UserManager.AddToRoleAsync
                                (UserManager.FindByName(user.UserName).Id, role);
                        }
                    }
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return result;
        }

        internal async Task<bool> UpdateUserAsync(PaskolUser user, string Password)
        {
            bool FinalRes = false;
            IdentityResult validEmail = await UserManager.UserValidator.ValidateAsync(user);
            if (!validEmail.Succeeded)
            {
                AddErrorsFromResult(validEmail, "Email");
            }

            IdentityResult validPass = null;

            if (!string.IsNullOrEmpty(Password))
            {
                validPass = await UserManager
                    .PasswordValidator.ValidateAsync(Password);
                if (validPass.Succeeded)
                {
                    user.PasswordHash =
                    UserManager.PasswordHasher.HashPassword(Password);
                }
                else
                {
                    AddErrorsFromResult(validPass, "Password");
                }
            }

            if ((validEmail.Succeeded && validPass == null) ||
                (validEmail.Succeeded && Password != string.Empty && validPass.Succeeded))
            {
                IdentityResult result = await UserManager.UpdateAsync(user);
                
                if (result.Succeeded)
                {
                    FinalRes = true;
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }

            return FinalRes;
        }

        [HttpPost]
        [Authorize(Roles = "SystemAdmin")]
        public ActionResult BlockActive(string ID, string returnUrl)
        {
            PaskolUser user = UserManager.Users.Single(u => u.Id == ID);

            if (user != null)
            {
                if (user.Status == UserStatus.Blocked)
                {
                    user.Status = UserStatus.Active;
                }
                else if (user.Status == UserStatus.Active)
                {
                    user.Status = UserStatus.Blocked;
                }

                UserManager.Update(user);
            }

            return Redirect(returnUrl);
        }

        internal void AddErrorsFromResult(IdentityResult result, string input = "")
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError(input, error);
            }
        }

        [HttpPost]
        public JsonResult GetImpersonatingUser(string StartWith)
        {
            var OriginalUser = HttpContext.User.GetUser();
            List<string> users = null;
            
            if (OriginalUser.UserType == UserType.Admin || User.GetOriginalType() == UserType.Admin)
            {
                users = UserManager.Users
                   .Where(u => u.UserName.StartsWith(StartWith) && (u.UserType == UserType.Artist ||
                                                                    u.UserType == UserType.Customer))
                   .Select(u => u.UserName)
                   .ToList();
            }
            else if (OriginalUser.UserType == UserType.ArtistAgent)
            {
                users = OriginalUser?.ArtistAgent?.Artists
                    .Where(u => (u.RelConfByArtist && u.RelConfByArtistAgent) && 
                           u.User.UserName.StartsWith(StartWith,true,CultureInfo.CurrentCulture))
                    .Select(u => u.User.UserName)
                    .ToList();
            }
            else if (User.GetOriginalType() == UserType.ArtistAgent)
            {
                users = UserManager.FindByEmail(User.GetOriginalEmail())?.ArtistAgent?.Artists
                    .Where(u => (u.RelConfByArtist && u.RelConfByArtistAgent) && 
                                 u.User.UserName.StartsWith(StartWith))
                    .Select(u => u.User.UserName)
                    .ToList();
            }
            
            return Json(users, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AJAXValidateAntiForgeryToken]
        public async Task RevertImpersonationAsync()
        {
            if (!HttpContext.User.IsImpersonating())
            {
                throw new Exception("Unable to remove impersonation because there is no impersonation");
            }

            var originalEmail = HttpContext.User.GetOriginalEmail();

            var originalUser = await UserManager.FindByEmailAsync(originalEmail);

            var impersonatedIdentity = await UserManager.CreateIdentityAsync(originalUser, DefaultAuthenticationTypes.ApplicationCookie);
            var authenticationManager = HttpContext.GetOwinContext().Authentication;

            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, impersonatedIdentity);
        }
        
        [HttpPost]
        [AJAXValidateAntiForgeryToken]
        public async Task ImpersonateUserAsync(string userName)
        {
            PaskolUser OriginalUser = null;
            // If user is already Impersonating then disconnect him first
            if (HttpContext.User.IsImpersonating())
            {
                await RevertImpersonationAsync();
                OriginalUser = this.UserManager.FindByEmail(User.GetOriginalEmail());
            }
            else
            {
                OriginalUser = HttpContext.User.GetUser();
            }
           
            PaskolUser impersonatedUser = await this.UserManager.FindByNameAsync(userName);
           

            if (impersonatedUser != null)
            {
                // If it's ok to impersonate
                if(ValidateImpersonating(OriginalUser, impersonatedUser))
                {
                    // Get original name and email
                    var originalEmail = OriginalUser.Email;
                    var originalUserName = OriginalUser.UserName;
                    var originalType = OriginalUser.UserType;
                    var RolesArray = RoleManager.Roles.Where(r => r.Users.Any(u => u.UserId == OriginalUser.Id)).Select(r => r.Name).ToArray();
                    string originalRoles = string.Join<string>(",", RolesArray);
                    // create identity with claim to original user for impersonate user
                    var impersonatedIdentity = await UserManager.CreateIdentityAsync(impersonatedUser,
                        DefaultAuthenticationTypes.ApplicationCookie);
                    // Set claims
                    impersonatedIdentity.AddClaim(new Claim("UserImpersonation", "true"));
                    impersonatedIdentity.AddClaim(new Claim("OriginalUsername", originalUserName));
                    impersonatedIdentity.AddClaim(new Claim("OriginalEmail", originalEmail));
                    impersonatedIdentity.AddClaim(new Claim("OriginalType", originalType.ToString()));
                    impersonatedIdentity.AddClaim(new Claim("OriginalRoles", originalRoles));
                    
                    var authenticationManager = HttpContext.GetOwinContext().Authentication;
                    authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, impersonatedIdentity);
                }
            }
        }

        private bool ValidateImpersonating(PaskolUser OriginalUser, PaskolUser ImpersonateUser)
        {
            try
            {
                return ((OriginalUser.UserType == UserType.Admin ||
                           User.GetOriginalType() == UserType.Admin) && (ImpersonateUser.UserType == UserType.Artist ||
                                                                        ImpersonateUser.UserType == UserType.Customer)) ||
                         ((OriginalUser.UserType == UserType.ArtistAgent ||
                          User.GetOriginalType() == UserType.ArtistAgent) && OriginalUser.ArtistAgent.Artists.Any(a => a.User.Id == ImpersonateUser.Id && 
                                                                                                                       a.RelConfByArtistAgent));
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}