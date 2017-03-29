using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Messaging
{
    public class RepositoryPagingResponse<T>
    {
        public int TotalResults { get; set; }
        public IEnumerable<T> Entities { get; set; }
    }
}
