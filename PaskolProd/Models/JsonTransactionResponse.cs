using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.Models
{
    public class JsonTransactionResponse
    {
        public string transactionID { get; set; }
        public string ValidationString { get; set; }
        public string URL { get; set; }
        public string ConfirmationKey { get; set; }
    }
}