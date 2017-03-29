using Model.Logs;
using System.Collections.Generic;
using Model;
using System;
using Repository.Messaging;

namespace Repository.Interfaces
{
    public interface ILogDataRepository : IGenericRepository<ActionOnLogData,int>
    {
        IEnumerable<UserMusicLog> UserMusicLogs(string Id);
        IEnumerable<UserOnEntityLog> UserArtistWatchLog(string Id);
        IEnumerable<UserOnEntityLog> UserWatchTagsLogs(string id);
        int LogCount(EntityType type, LogActionType action, DateTime? start, DateTime? end);
        RepositoryPagingResponse<LeadMusicLog> GetMusicLogByAction(LogActionType action, DateTime start, DateTime end, int skip, int take);
    }
}
