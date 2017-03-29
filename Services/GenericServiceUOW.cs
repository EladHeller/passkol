using System;
using Repository;
using Services.Messaging.Responses;
using Infrastructure.Domain;
using Services.Messaging.Requests;
using Repository.Interfaces;
using Repository.UnitOfWork;
using Model.Users;
using System.Collections.Generic;

namespace Services
{
    public class GenericServiceUOW<T, TID>  where T : class, IAggregateRoot
    {
        protected IGenericRepository<T, TID> _rep { get; set; }
        protected IUnitOfWork _uow;

        public GenericServiceUOW(IGenericRepository<T, TID> repository, 
            IUnitOfWork uow)
        {
            this._uow = uow;
            this._rep = repository;
        }
        
        public virtual PagingResponse<T> Get(PagingRequest pagingRequest)
        {
            PagingResponse<T> res = new PagingResponse<T>();

            try
            {
                var repopositoryRes = this._rep.Get(
                    (pagingRequest.Page - 1) * pagingRequest.ResultInPage,
                    pagingRequest.ResultInPage);
                res.Entities = repopositoryRes.Entities;
                res.CurrPage = pagingRequest.Page;
                res.Success = true;
                res.ResultsInPage = pagingRequest.ResultInPage;
            }
            catch (Exception e)
            {
                res.Success = false;
                res.Message = e.ToString();
            }
            return res;
        }
        public virtual ResponseBase AddRange(IEnumerable<T> items)
        {
            ResponseBase res = new ResponseBase();
            res.Success = true;
            foreach (var item in items)
            {
                res = ValidateItem(item);
                if (!res.Success)
                {
                    break;
                }
            }

            if (res.Success)
            {
                try
                {
                    this._rep.AddRange(items);
                    this._uow.Commit();

                    res.Success = true;
                }
                catch (Exception e)
                {
                    res.Success = false;
                    res.Message = e.ToString();
                }
            }
            return res;
        }

        public virtual ResponseBase Add(T item)
        {
            ResponseBase res = ValidateItem(item);

            if (!res.Success)
            {
                return res;
            }

            try
            {
                this._rep.Add(item);
                this._uow.Commit();

                res.Success = true;
            }
            catch (Exception e)
            {
                res.Success = false;
                res.Message = e.ToString();
            }

            return res;
        }

        public virtual ResponseBase Delete(T item)
        {
            ResponseBase res = new ResponseBase();

            try
            {
                this._rep.Delete(item);
                this._uow.Commit();

                res.Success = true;
            }
            catch (Exception e)
            {
                res.Success = false;
                res.Message = e.ToString();
            }

            return res;
        }

        public virtual ResponseBase DeleteBy(TID id)
        {
            var EntityTodelete = this.GetByID(id);
            if (EntityTodelete.Success && EntityTodelete.Entity != null)
            {
                return this.Delete(EntityTodelete.Entity);
            }
            return new ResponseBase() { Success = false, Message = "לא נמצאה רשמוה למחיקה" };
        }

        public virtual EntityAllResponse<T> GetAll()
        {
            EntityAllResponse<T> res = new EntityAllResponse<T>();

            try
            {
                res.Entities = this._rep.GetAll();
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Success = false;
                res.Message = e.Message;
            }

            return res;
        }

        public virtual EntityResponse<T> GetByID(TID id)
        {
            EntityResponse<T> res = new EntityResponse<T>();

            try
            {
                res.Entity = this._rep.GetByID(id);
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Success = false;
                res.Message = e.Message;
            }

            return res;
        }

        public virtual ResponseBase Update(T item)
        {
            ResponseBase res = ValidateItem(item);
            if (!res.Success)
            {
                return res;
            }

            try
            {
                this._rep.Update(item);
                this._uow.Commit();
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Success = false;
                res.Message = e.ToString();
            }

            return res;
        }

        public virtual ResponseBase ValidateItem(T item)
        {
            return new ResponseBase() { Message = "", Success = true };
        }

    }
}
