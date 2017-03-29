using Model.Logs;
using PaskolProd.Authentication;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Model.Controllers.Paskol
{
    public class LogController : Controller
    {
        private ReportService _rptSrv;
        private SearchLogService _srchSrv;
        private TagService _tgSrv;

        public LogController()
        {
            _rptSrv = ServiceLocator.GetService<ReportService>();
            _srchSrv = ServiceLocator.GetService<SearchLogService>();
            _tgSrv = ServiceLocator.GetService<TagService>();
        }

        [HttpPost]
        public ActionResult Log(ActionOnLogData logData)
        {
            if (User.Identity.IsAuthenticated)
            {
                logData.UserID = User.GetUser().Id;
            }
            logData.Value = 1;
            logData.DateTime = DateTime.Now;
            var res = _rptSrv.Add(logData);
            return Json(res);
        }
        [HttpPost]
        public ActionResult Logs(IEnumerable<ActionOnLogData> logsData)
        {
            foreach (var log in logsData)
            {
                if (User.Identity.IsAuthenticated)
                {
                        log.UserID = User.GetUser().Id;
                }
                log.Value = 1;
                log.DateTime = DateTime.Now;
            }
            var res = _rptSrv.AddRange(logsData);
            return Json(res);
        }
        [HttpPost]
        [Authorize]
        public ActionResult SearchLog(string searchString, IEnumerable<Guid> tags, int resultCount)
        {
            var logData = new SearchLog();
            var getTagsRes = _tgSrv.GetAll();
            if (getTagsRes.Success)
            {
                if (tags != null)
                {
                    logData.Tags =
                    getTagsRes.Entities.
                    Where(t => tags.Any(tgId => tgId == t.ID)).
                    ToList();
                }
                
                logData.SearchString = searchString;
                logData.ResultCount = resultCount;
                logData.UserID = User.GetUser().Id;
                logData.DateTime = DateTime.Now;
                var res = _srchSrv.Add(logData);

                return Json(res);
            }
            else
            {
                return Json(new { Success = false, Message = getTagsRes.Message});
            }
        }
    }
}