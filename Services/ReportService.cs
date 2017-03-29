using Repository.UnitOfWork;
using Repository.Interfaces;
using Services.Messaging.Responses;
using System;
using Model.Logs;
using Model;
using Services.Messaging.Requests;
using Repository.Messaging;

namespace Services
{
    public class ReportService : GenericServiceUOW<ActionOnLogData, int>
    {
        private new ILogDataRepository _rep;
        private new IUnitOfWork _uow;
        private IPurchaseRepository _purchRep;

        public ReportService(IPurchaseRepository purchRep ,ILogDataRepository Logrep, IUnitOfWork uow) : base(Logrep, uow)
        {
            this._rep = Logrep;
            this._uow = uow;
            _purchRep = purchRep;
        }
        
        public EntityResponse<int> GetLogsCountForEntityAction(EntityType type, LogType action, DateTime? start, DateTime? end)
        {
            var res = new EntityResponse<int>();
            res.Success = true;
            try
            {
                switch (action)
                {
                    case LogType.Purchase:
                        res.Entity = _purchRep.GetMusicsPurchaseCount(start, end);
                        break;
                    case LogType.MusicView:
                        res.Entity = _rep.LogCount(type, LogActionType.Watch, start, end);
                        break;
                    default:
                        res.Success = false;
                        res.Message = "This log type isn't supported!";
                        break;
                }
            }
            catch (Exception e)
            {
                res.Success = false;
                res.Message = e.ToString();
            }
            return res;
        }

        public PagingResponse<LeadMusicLog> GetMusicLogByAction(LogType action, DateTime start, DateTime end, PagingRequest pageReq)
        {
            var res = new PagingResponse<LeadMusicLog>(pageReq);
            res.Success = true;
            try
            {
                RepositoryPagingResponse<LeadMusicLog> dbRes = new RepositoryPagingResponse<LeadMusicLog>();
                switch (action)
                {
                    case LogType.Purchase:
                        dbRes = _purchRep.GetMusicsPurchase(start, end, (pageReq.Page - 1) * pageReq.ResultInPage, pageReq.ResultInPage);
                        break;
                    case LogType.MusicView:
                        dbRes = _rep.GetMusicLogByAction(LogActionType.Watch, start, end , (pageReq.Page -1) * pageReq.ResultInPage, pageReq.ResultInPage);
                        break;
                    default:
                        res.Success = false;
                        res.Message = string.Format("Action - {0}, not supported for GetMusicLogByAction method", action.ToString());
                        break;
                }
                res.TotalResults =  dbRes.TotalResults;
                res.Entities = dbRes.Entities;
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Success = false;
                res.Message = e.ToString();
            }
            return res;
        }
        public LogsForUserResponse LogsForUser(string Id)
        {
            LogsForUserResponse res = new LogsForUserResponse();

            try
            {
                res.UserMusicLogs = this._rep.UserMusicLogs(Id);
                res.UserWatchArtistLogs = this._rep.UserArtistWatchLog(Id);
                res.UserWatchTagsLogs = this._rep.UserWatchTagsLogs(Id);
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Message = e.Message;
                res.Success = false;
            }
            
            return res;
        }
    }
}
