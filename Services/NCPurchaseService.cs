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
    public class NCPurchaseService : GenericServiceUOW<NCPurchase, string>
    {
        private IPurchaseRepository _pRep;
        public NCPurchaseService(IPurchaseRepository pRep ,INCPurchaseRepository NCRep, IUnitOfWork uow) :base(NCRep,uow)
        {
            this._pRep = pRep;
        }
        
        public PagingResponse<NCPurchase> Get(PagingRequest pageRequest, DateTime s, DateTime e)
        {
            PagingResponse<NCPurchase> res = new PagingResponse<NCPurchase>();

            try
            {
                var repositoryRes = (this._rep as INCPurchaseRepository).Get(pageRequest.ResultInPage,
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

        public ResponseBase SaveByStatus(string note, NCAction action, string id)
        {
            ResponseBase res = new ResponseBase();
            try
            {
                switch (action)
                {
                    case NCAction.remove:
                        this.DeleteBy(id);
                        this._pRep.Delete(this._pRep.GetByID(id));
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
