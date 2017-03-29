using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Interfaces;
using Repository.UnitOfWork;
using Services.Messaging.Responses;

namespace Services
{
    public class PermissionService : GenericServiceUOW<Permission, Guid>
    {
        public PermissionService(IPermmsionRepository repository, IUnitOfWork uow) : base(repository, uow)
        {
        }



        public ResponseBase UpdateAndAddList(IEnumerable<Permission> lstUpdate, IEnumerable<Permission> lstAdd)
        {
            var res = new ResponseBase();
            try
            {
                foreach (var item in lstUpdate)
                {
                    res = ValidateItem(item);
                    if (!res.Success)
                    {
                        break;
                    }
                    _rep.Update(item);
                }
                foreach (var item in lstAdd)
                {
                    res = ValidateItem(item);
                    if (!res.Success)
                    {
                        break;
                    }
                    _rep.Add(item);
                }
                if (res.Success)
                {
                    _uow.Commit();
                }
            }
            catch (Exception e)
            {
                res.Success = false;
                res.Message = e.ToString();
            }

            return res;
        }
    }
}
