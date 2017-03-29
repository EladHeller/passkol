using Microsoft.AspNet.Identity.EntityFramework;
using Model.Users;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Model.Authentication
{
    public class PaskolUserStore : UserStore<PaskolUser>
    {
        public PaskolUserStore(DbContext context)
            : base(context)
        {
        }

        public new IQueryable<PaskolUser> Users { get { return base.Users.Where(u => u.NCRegisterLog == null); } }
        public override Task<PaskolUser> FindByIdAsync(string userId)
        {
            return Users
                .Include(u => u.Customer)
                .Include(u => u.ArtistAgent)
                .Include(a => a.Artist)
                .Include(u => u.Roles)
                .Include(u => u.Claims)
                .Include(u => u.Logins).FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}