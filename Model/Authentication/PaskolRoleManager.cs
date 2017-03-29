using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Authentication
{
    public class PaskolRoleManager : RoleManager<PaskolRole>, IDisposable
    {
        public PaskolRoleManager(RoleStore<PaskolRole> store)
            : base(store)
        {
        }

        public static PaskolRoleManager Create(IdentityFactoryOptions<PaskolRoleManager> options,
            IOwinContext context)
        {
            return new PaskolRoleManager(new RoleStore<PaskolRole>(context.Get<PaskolDbContext>()));
        }
    }
}
