using Microsoft.AspNet.Identity;
using Model.Users;
using Services;
using Services.Messaging.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace PaskolProd.Authentication
{
    public static class PrincipalExtensions
    {
        public static PaskolUser GetUser(this IPrincipal user)
        {
            PaskolUser UserToReturn = null;
            if (user.Identity.IsAuthenticated)
            {
                return ServiceLocator.GetService<UsersService>().GetByID(user.Identity.GetUserId()).Entity;
            }
            return UserToReturn;
        }

        public static bool IsImpersonating(this IPrincipal principal)
        {
            if (principal == null)
            {
                return false;
            }

            var claimsPrincipal = principal as ClaimsPrincipal;
            if (claimsPrincipal == null)
            {
                return false;
            }


            return claimsPrincipal.HasClaim("UserImpersonation", "true");
        }

        public static UserType? GetOriginalType(this IPrincipal principal)
        {
            var resType = GetOriginalDetail(principal, "OriginalType");
            if (string.IsNullOrEmpty(resType))
            {
                return null;
            }

            return (UserType)Enum.Parse(typeof(UserType), resType);
        }

        public static bool IsUserInRole(this IPrincipal principal, string role)
        {
            if (principal.IsInRole(role))
            {
                return true;
            }
            else if (principal.IsImpersonating())
            {
                var originalRoles = HttpContext.Current.User.GetOriginalRoles().Split(',');
                if (originalRoles.Any(r => r == role))
                {
                    return true;
                }
            }

            return false;
        }

        public static String GetOriginalEmail(this IPrincipal principal)
        {
            return GetOriginalDetail(principal, "OriginalEmail");
        }

        public static String GetOriginalName(this IPrincipal principal)
        {
            return GetOriginalDetail(principal, "OriginalUsername");
        }

        public static string GetOriginalRoles(this IPrincipal principal)
        {
            return GetOriginalDetail(principal, "OriginalRoles");
        }

        private static string GetOriginalDetail(IPrincipal principal,string claimName)
        {
            if (principal == null)
            {
                return String.Empty;
            }

            var claimsPrincipal = principal as ClaimsPrincipal;
            if (claimsPrincipal == null)
            {
                return String.Empty;
            }

            if (!claimsPrincipal.IsImpersonating())
            {
                return String.Empty;
            }

            var originalUsernameClaim = claimsPrincipal.Claims.SingleOrDefault(c => c.Type == claimName);

            if (originalUsernameClaim == null)
            {
                return String.Empty;
            }
            return originalUsernameClaim.Value;
        }
        
    }
}