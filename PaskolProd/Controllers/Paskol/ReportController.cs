using Model.Filters;
using Model.Logs;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Model.Controllers.Paskol
{
    public class ReportController : Controller
    {
        public ReportService ReportService { get; set; }
        public ReportController()
        {
            ReportService = ServiceLocator.GetService<ReportService>();
        }
        [HttpPost]
        [AJAXValidateAntiForgeryToken]
        public void CreateLog(ActionOnLogData logData)
        {
            if (ModelState.IsValid)
            {
                ReportService.Add(logData);
            }
        }
    }
}