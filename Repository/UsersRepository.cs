using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Repository.Messaging;
using Infrastructure.Domain;
using Repository;
using Repository.UnitOfWork;
using Model.Users;
using System.Data.Entity;

namespace Repository
{
    public class UsersRepository : GenericRepository<PaskolUser,string>, IUsersRepository
    {
        public UsersRepository(IUnitOfWork uof) : base(uof)
        {
        }

        new internal IQueryable<PaskolUser> dbSet
        {
            get
            {
                return base.dbSet.Where(u => u.NCRegisterLog == null);
            }
        }

        public override IEnumerable<PaskolUser> GetAll()
        {
            return this.dbSet.ToList();
        }

        public PaskolUser GetArtistByName(string userName)
        {
            return dbSet.Include("Artist").FirstOrDefault(art=>art.UserName == userName);
        }

        public PaskolUser GetArtistByPageUrl(string urlPage)
        {
            return dbSet.
                Include("Artist.ArtistMusics.Tags").
                FirstOrDefault(u =>
                    u.Artist != null &&
                    u.Artist.PageUrl == urlPage);
        }

        public PaskolUser GetByName(string name)
        {
            return dbSet.FirstOrDefault(u => u.UserName == name);
        }

        public PaskolUser GetByEmail(string email)
        {
            return dbSet.FirstOrDefault(u => u.Email == email);
        }

        public IEnumerable<PaskolUser> GetAllBy(UserType ut)
        {
            return this.dbSet.Where(u => u.UserType == ut);
        }
    }
}
