using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Users;
using Infrastructure.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Logs;
using System.Security.Claims;
using Microsoft.AspNet.Identity;

namespace Model.Users
{
    public class PaskolUser : IdentityUser, IAggregateRoot
    {
        public DateTime RegisteredDate { get; set; }
        public virtual Artist Artist { get; set; }
        public virtual ArtistAgent ArtistAgent { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<Music> FavouriteMusics { get; set; }
        public virtual NCRegister NCRegisterLog  { get; set; }
        public UserStatus Status { get; set; }
        public string Note { get; set; }
        public UserType UserType{ get; set; }
        public virtual ICollection<SearchLog> SearchLogs { get; set; }
        public bool RecoverPasswordLockOut
        {
            get
            {
                return RecoverPasswordReleased >= DateTime.Now; 
            }
        }
        public int RecoverPasswordAttempt{ get; set; }
        public DateTime? RecoverPasswordReleased { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<PaskolUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}