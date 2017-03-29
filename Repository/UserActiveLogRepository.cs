using System.Collections.Generic;
using System.Linq;
using System;
using Repository.UnitOfWork;
using Repository.Interfaces;
using Model.Logs;
using Model.Users;

namespace Repository
{
    public class UserActiveLogRepository : GenericRepository<UserActiveLog, string> , IUserActiveLogRepository
    {
        public UserActiveLogRepository(IUnitOfWork uof) : base(uof)
        {
        }

        public int ActiveUserByDateAndType(DateTime s, DateTime e, UserType ut)
        {
            return 
                this.dbSet
                .Where(u => u.User.UserType == ut && u.ActionDate >= s && u.ActionDate <= e)
                .Select(l => l.UserId)
                .Distinct()
                .Count();
        }
    }
}
