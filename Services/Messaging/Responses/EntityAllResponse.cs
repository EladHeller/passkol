using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Users;

namespace Services.Messaging.Responses
{
    public class EntityAllResponse<T> : ResponseBase
    {
        public IEnumerable<T> Entities { get; set; }

        public bool HasData 
        { 
            get
            {
                return this.Entities.Count() > 0;
            }
        }
    }
}
