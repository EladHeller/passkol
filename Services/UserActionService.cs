using Repository.UnitOfWork;
using Repository.Interfaces;
using Services.Messaging.Responses;
using System;
using Services.Messaging.Requests;
using Repository;
using Model.Users;
using Model.Confirm;
using Model.Purchase;
using Model.Auctions;
using System.Collections.Generic;
using Model.Logs;

namespace Services
{
    public class UserActionService : GenericServiceUOW<UserActiveLog, string>
    {
        private IUsersRepository _userRep;
        private IUserActiveLogRepository _UserActiveLogRep;

        public UserActionService(IUserActiveLogRepository UserActiveLogRep, IUnitOfWork uow) 
            : base(UserActiveLogRep,uow)
        {
            //this._userRep = userRep;
            this._UserActiveLogRep = UserActiveLogRep;
        }

        public EntityResponse<int> GetByDateAndType(DateTime s, DateTime e, UserType ut)
        {
            EntityResponse<int> res = new EntityResponse<int>();

            try
            {
                res.Entity =
                    this._UserActiveLogRep.ActiveUserByDateAndType(s, e, ut);
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = ex.Message;
                throw;
            }
            return res;
        }
    }
}
