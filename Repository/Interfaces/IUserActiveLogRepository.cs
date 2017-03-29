using Model;
using Model.Auctions;
using Model.Logs;
using Model.Users;
using Repository.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IUserActiveLogRepository : IGenericRepository<UserActiveLog, string>
    {
        int ActiveUserByDateAndType(DateTime s, DateTime e, UserType ut);
    }
}
