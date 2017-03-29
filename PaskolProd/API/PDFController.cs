using Model.Purchase;
using Services;
using Services.Messaging.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PDFws.API
{
    public class PDFController : ApiController
    {
        private FileSystemService _service;

        public PDFController()
        {
            this._service = ServiceLocator.GetService<FileSystemService>();
        }

        // GET: api/PDF
        public ResponseBase UpdateUserPurchaseReport(Purchase p)
        {
            Model.File newPDF = new Model.File()
            {
                FileType = Model.FileType.PDF,
                UserId = p.Customer.Id
            };

            return  _service.StoreFile(newPDF);
        }
    }
}
