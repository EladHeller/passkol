using Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IUsersRepository : IGenericRepository<PaskolUser, string>
    {
        IEnumerable<PaskolUser> GetAllBy(UserType ut);
        PaskolUser GetArtistByName(string userName);
        PaskolUser GetByName(string name);
        PaskolUser GetByEmail(string email);
        PaskolUser GetArtistByPageUrl(string urlPage);
    }
}
