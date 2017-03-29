using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Messaging.Requests;

namespace Services.Messaging.Responses
{
    public class PagingResponse<T> : EntityAllResponse<T>
    {
        public PagingResponse()
        {

        }
        public PagingResponse(PagingRequest pageReq)
        {
            CurrPage = pageReq.Page;
            ResultsInPage = pageReq.ResultInPage;
        }

        public int TotalResults { get; set; }
        public int CurrPage { get; set; }
        public int ResultsInPage { get; set; }
        public int TotalPages
        {
            get
            {
                int remind = (TotalResults % ResultsInPage);
                return ((remind > 0) ? 1 : 0) + (TotalResults / ResultsInPage);
            }
        }
    }
}
