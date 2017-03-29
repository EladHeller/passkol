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
using Services.Helpers;

namespace Services
{
    public class NCRegisterService : GenericServiceUOW<NCRegister, string>
    {
        private INCRegisterRepository _NCRegisterRep;
        private IUsersRepository _userRep;

        public NCRegisterService(IUsersRepository userRep, INCRegisterRepository NCRegisterRep, IUnitOfWork uow) :base(NCRegisterRep,uow)
        {
            this._userRep = userRep;
            this._NCRegisterRep = NCRegisterRep;
        }
        
        public PagingResponse<NCRegister> Get(PagingRequest pageRequest, DateTime s, DateTime e, NCUserType? nCUserType)
        {
            PagingResponse<NCRegister> res = new PagingResponse<NCRegister>();

            try
            {
                var repositoryRes = (this._rep as INCRegisterRepository).Get(pageRequest.ResultInPage,
                    (pageRequest.Page - 1) * pageRequest.ResultInPage, s, e, nCUserType);

                res.Entities = repositoryRes.Entities;
                res.TotalResults = repositoryRes.TotalResults;
                res.ResultsInPage = pageRequest.ResultInPage;
                res.CurrPage = pageRequest.Page;
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = ex.ToString();
            }

            return res;
        }

        public ResponseBase SaveByStatus(string note, NCAction action, string id)
        {
            ResponseBase res = new ResponseBase();
            try
            {
                switch (action)
                {
                    case NCAction.remove:
                        this.DeleteBy(id);
                        this._userRep.Delete(this._userRep.GetByID(id));
                        break;
                    case NCAction.save:
                        var resId = this._rep.GetByID(id);
                        resId.Note = note;
                        this._rep.Update(resId);
                        break;
                }
                this._uow.Commit();
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Success = false;
            }

            return res;
        }
    }
}
