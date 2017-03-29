using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaskolProd.Models
{
    public class JsonTransactionDetails
    {
        public string UserKey { get; set; }
        public string terminal { get; set; }
        public string user { get; set; }
        public string password { get; set; }
        public string GoodURL { get; set; }
        public string Total { get; set; }
        public string CssURL { get; set; }
        public string ErrorURL { get; set; }
        public string FeedbackDataTransferMethod { get; set; }
        public bool feedbackOnTop { get; set; }
        public UserData UserData { get; set; } 
    }

    public class UserData
    {
        public string UserData1 { get; set; }
        public string UserData2 { get; set; }
        public string UserData3 { get; set; }
        public string UserData4 { get; set; }
        public string UserData5 { get; set; }
        public string UserData6 { get; set; }
        public string UserData7 { get; set; }
        public string UserData8 { get; set; }
    }

    public class GetTransactReq
    {
        public string terminal { get; set; }
        public string user { get; set; }
        public string password { get; set; }
        public string TransactionId { get; set; }
    }

    public class ValidateTransactReq
    {
        public string ConfirmationKey { get; set; }
        public string UniqueKey { get; set; }
        public string Total { get; set; }
    }

    public class GetTransactRes
    {
        public ResultData ResultData { get; set; }
        public UserData UserData { get; set; }
    }

    public class ResultData
    {
        public string DebitTotal { get; set; }
    }
}                                         
                                             