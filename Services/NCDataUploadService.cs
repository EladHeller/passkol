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
    public class NCDataUploadService : GenericServiceUOW<NCDataUpload, int>
    {
        private INCDataUploadRepository _NCRep;
        private IMusicsRepository _mRep;
        public NCDataUploadService(IMusicsRepository mRep, INCDataUploadRepository NCRep, IUnitOfWork uow) 
            :base(NCRep,uow)
        {
            this._mRep = mRep;
            this._NCRep = NCRep;
        }
        
        public PagingResponse<NCDataUpload> Get(PagingRequest pageRequest, DateTime s, DateTime e)
        {
            PagingResponse<NCDataUpload> res = new PagingResponse<NCDataUpload>();

            try
            {
                var repositoryRes = (this._rep as INCDataUploadRepository).Get(pageRequest.ResultInPage,
                    (pageRequest.Page - 1) * pageRequest.ResultInPage, s, e);

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

        public ResponseBase SaveByStatus(string note, NCAction action, int id)
        {
            ResponseBase res = new ResponseBase();
            try
            {
                switch (action)
                {
                    case NCAction.remove:
                        this.DeleteBy(id);
                        this._mRep.Delete(this._mRep.GetByID(id));
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
