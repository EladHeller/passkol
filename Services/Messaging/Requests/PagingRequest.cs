using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Messaging.Requests
{
    public class PagingRequest : IBaseRequqest
    {
        public PagingRequest(int page, int resultInPage)
        {
            this.Page = page;
            this.ResultInPage = resultInPage;
        }
        public int Page { get; set; }
        public int ResultInPage { get; set; }
        public IBaseRequqest InnerRequest { get; set; }
    }
}
