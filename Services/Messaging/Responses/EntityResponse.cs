using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Users;

namespace Services.Messaging.Responses
{
    public class EntityResponse<T> : ResponseBase
    {
        public T Entity { get; set; }
    }
}
