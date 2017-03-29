using Model;
using Repository.Interfaces;
using Repository.UnitOfWork;
using Services.Messaging.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PermissionPropertyService : GenericServiceUOW<PermissionProperty, Guid>
    {
        public PermissionPropertyService(IPermissionPropertyRepository rep, IUnitOfWork uow) :base(rep, uow)
        { }

        public EntityAllResponse<PermissionProperty> GetAll(Guid categoryId)
        {
            EntityAllResponse<PermissionProperty> res = new EntityAllResponse<PermissionProperty>();
            res.Success = false;
            try
            {
                res.Entities = (_rep as IPermissionPropertyRepository).GetAll(categoryId);
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Message = e.ToString();
            }

            return res;
        }

        public EntityResponse<PermissionProperty> GetByID(Guid catId, bool includeValues)
        {
            EntityResponse<PermissionProperty> res = new EntityResponse<PermissionProperty>();
            res.Success = false;

            try
            {
                res.Entity = (_rep as IPermissionPropertyRepository).GetByID(catId, includeValues);
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Message = e.ToString();
            }

            return res;
        }

        public override ResponseBase Update(PermissionProperty item)
        {
            var getRes = this.GetByID(item.ID, true);
            if (!getRes.Success)
            {
                return getRes;
            }
            var dbPerProp = getRes.Entity;
            dbPerProp.IsActive = item.IsActive;
            dbPerProp.Name = item.Name;
            dbPerProp.ViewOrder = item.ViewOrder;
            if (item.PropertyValues != null)
            {
                if (dbPerProp.PropertyValues == null)
                {
                    dbPerProp.PropertyValues = item.PropertyValues;
                }
                else
                {
                    dbPerProp.PropertyValues.RemoveAll(dbVal => !item.PropertyValues.Any(val => val.ID == dbVal.ID));
                    item.PropertyValues.ForEach(propVal=> UpdateDbPropertyValue(dbPerProp, propVal));
                }
            }
            return base.Update(dbPerProp);
        }

        private void UpdateDbPropertyValue(PermissionProperty dbProp, PermissionPropertyValue propVal)
        {
            var dbVal = dbProp.PropertyValues.FirstOrDefault(x => x.ID == propVal.ID);
            if (dbVal==null)
            {
                propVal.ID = Guid.NewGuid();
                dbProp.PropertyValues.Add(propVal);
            }
            else
            {
                dbVal.IsActive = propVal.IsActive;
                dbVal.Name = propVal.Name;
                dbVal.ViewOrder = propVal.ViewOrder;
            }
        }
    }
}