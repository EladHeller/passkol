using Repository;
using Services.Messaging.Requests;
using Services.Messaging.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IGenericService<T, TID>
    {
        EntityAllResponse<T> GetAll();
        EntityResponse<T> GetByID(TID id);
        PagingResponse<T> GetPage(PagingRequest pageingREquest);
        ResponseBase Add(T item);
        ResponseBase AddRange(IEnumerable<T> item);
        ResponseBase Update(T item);
        ResponseBase Delete(T item);
        ResponseBase ValidateItem(T item);
    }
}
