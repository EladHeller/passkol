using Infrastructure.Domain;
using Repository;
using Services.Messaging.Responses;
using System;
using Infrastructure;
using Model.Users;
using Model;
using System.Threading.Tasks;
using Repository.Interfaces;
using Repository.UnitOfWork;
using System.Collections;
using System.Collections.Generic;

namespace Services
{
    public class TagService : GenericServiceUOW<Tag, Guid>
    {
        private new ITagRepository _rep;

        public TagService(ITagRepository rep, IUnitOfWork uow) : base(rep, uow)
        {
            this._rep = rep;
            this._uow = uow;
        }

        public EntityResponse<Tag> GetNewInSystemTag()
        {
            EntityResponse<Tag> res = new EntityResponse<Tag>();

            try
            {
                res.Entity = this._rep.GetNewInSystemTag();
                if (res.Entity == null)
                {
                    res.Success = false;
                    res.Message = "התיוג חדש במערכת לא נמצא";
                }
                else
                {
                    res.Success = true;
                }
            }
            catch (Exception e)
            {
                res.Success = false;
                res.Message = e.Message;
            }

            return res;
        }

        public EntityAllResponse<Tag> GetAllByParentId(Guid? parentId)
        {
            EntityAllResponse<Tag> res = new EntityAllResponse<Tag>();

            try
            {
                res.Entities = this._rep.GetAllByParentId(parentId);
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Success = false;
                res.Message = e.Message;
            }

            return res;
        }

        public EntityAllResponse<Tag> GetBySearch(string tagName)
        {
            EntityAllResponse<Tag> res = new EntityAllResponse<Tag>();

            try
            {
                res.Entities = this._rep.GetBySearch(tagName);
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Success = false;
                res.Message = e.Message;
            }

            return res;
        }

        public EntityResponse<Tag> GetByID(Guid Id, ModelGetNavigtorOptions getParent = ModelGetNavigtorOptions.None,
            ModelGetNavigtorOptions getChildren = ModelGetNavigtorOptions.None)
        {
            EntityResponse<Tag> res = new EntityResponse<Tag>();

            try
            {
                res.Entity = this._rep.GetByID(Id, getParent, getChildren);
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Success = false;
                res.Message = e.Message;
            }

            return res;
        }

        public override ResponseBase Update(Tag tag)
        {
            var getTagRes = GetByID(tag.ID);
            if (!getTagRes.Success)
            {
                return getTagRes;
            }

            ResponseBase res = new ResponseBase();
            var dbTag = getTagRes.Entity;
            dbTag.IsPublicTag = tag.IsPublicTag;
            dbTag.Name = tag.Name;
            dbTag.ViewOrder = tag.ViewOrder;

            try
            {
                this._rep.Update(dbTag);
                this._uow.Commit();

                res.Success = true;
            }
            catch (Exception e)
            {
                res.Success = false;
                res.Message = e.Message;
            }

            return res;
        }

        public override ResponseBase Delete(Tag Tag)
        {
            ResponseBase res = new ResponseBase();
            res.Success = false;

            if (!Tag.IsCanDelete)
            {
                res.Message = "לא ניתן למחוק קטגוריה זו!";
            }
            else
            {
                try
                {
                    this._rep.Delete(Tag);
                    this._uow.Commit();
                    res.Success = true;
                }
                catch (Exception e)
                {
                    res.Message = e.Message;
                }
            }
            
            return res;
        }

        public override ResponseBase Add(Tag tag)
        {
            // Base validation
            ResponseBase res = ValidateTag(tag);

            if (!res.Success)
            {
                return res;
            }
            tag.ID = Guid.NewGuid();
            
            try
            {
                this._rep.Add(tag);
                this._uow.Commit();
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Success = false;
                res.Message = e.Message;
            }

            return res;
        }

        public EntityAllResponse<Tag> GetTagList(IEnumerable<Guid> tagIds)
        {
            var res = new EntityAllResponse<Tag>();
            try
            {
                res.Entities = this._rep.GetTagList(tagIds);
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Message = e.ToString();
                res.Success = false;
            }
            return res;
        }

        public ResponseBase ValidateTag(Tag tag)
        { 
            ResponseBase res = new ResponseBase();

            res.Success = false;
            res.Message = "";

            if ((tag.Level > TagLevel.Root && tag.ParentTagID == null))
            {
                res.Message += "יש לקשר קטגורית אב.\r\n";
            }

            if (tag.Level == TagLevel.Bottom)
            {
                tag.TagList = null;
            }

            if (res.Message.Length == 0)
            {
                res.Success = true;
            }

            return res;
        }
    }
}
