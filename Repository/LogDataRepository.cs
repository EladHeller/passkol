using Repository.Interfaces;
using System.Collections.Generic;
using Model.Logs;
using Repository.UnitOfWork;
using Model;
using System.Linq;
using System.Data.Entity;
using System;
using Repository.Messaging;

namespace Repository
{
    public class LogDataRepository : GenericRepository<ActionOnLogData, int>, ILogDataRepository
    {
        private string queryLeadMusicFormat = @"select ID as MusicId, 
		                    UserName as ArtistName,
		                    HebrewName,
		                    Count,
		                    isnull(avg(p.PurchaseCost),0) as AvgCost,
		                    isnull(sum(p.PurchaseCost),0) as SumCosts,
		                    isnull(sum(p.ArtistEarn),0) as ArtistEarn,
		                    isnull(sum(p.PasskolEarn),0) as PasskolEarn
                    from (select m.ID,u.UserName, m.HebrewName, sum(l.Value) Count
                    from ActionOnLogDatas l join musics m on m.Id = l.EntityId
				                    join AspNetUsers u on u.Id = m.ArtistID
                    where ActionType = {0} and EntityType = 1
                    and l.DateTime > '{1}' and l.DateTime < '{2}'
                    group by u.UserName, m.HebrewName,m.ID) a
                    left join Purchases p on p.MusicID = a.ID
                    group by a.UserName, a.HebrewName,a.ID, a.Count
                    Order by a.Count desc";

        public LogDataRepository(IUnitOfWork uow) : base(uow)
        {
        }

        public IEnumerable<UserMusicLog> UserMusicLogs(string Id)
        {
            return ((PaskolDbContext)_uow.context).Database.SqlQuery<UserMusicLog>
                (string.Format(@"select [UserName] as ArtistName, 
                                        [HebrewName] as MusicName, 
                                        isnull([0],0) as WatchCount, 
                                        isnull([1],0) as ListenCount
                                 from
                                 (
                                     select  u.UserName, m.HebrewName,ActionType, sum(value)sum 
                                     from ActionOnLogDatas l join musics m on m.Id = l.EntityId
				                                     join AspNetUsers u on u.Id = m.ArtistID
                                     where l.userid = '{0}' AND l.EntityType = {1}
                                     group by ActionType, m.HebrewName, u.UserName
                                  )a
                                  pivot
                                  (
	                                  sum(sum)
	                                  for ActionType in ([0],[1])
                                  )piv;", Id, (int)EntityType.Music))
                    .ToList();
             
        }

        public RepositoryPagingResponse<LeadMusicLog> GetMusicLogByAction(LogActionType action, DateTime start, DateTime end, int skip, int take)
        {
            RepositoryPagingResponse<LeadMusicLog> res = new RepositoryPagingResponse<LeadMusicLog>();
            string queryString = null;
            queryString = String.Format(queryLeadMusicFormat, (int)action, start.ToString("yyyy-MM-dd HH:mm:ss"), end.ToString("yyyy-MM-dd  HH:mm:ss"));
            var query = ((PaskolDbContext)_uow.context).Database.SqlQuery<LeadMusicLog>(queryString);
            res.TotalResults = query.Count();
            res.Entities = query.OrderByDescending(l => l.Count).Skip(skip).Take(take);
            return res;
        }

        public int LogCount(EntityType type,LogActionType action, DateTime? start, DateTime? end)
        {
            IQueryable<ActionOnLogData> query = dbSet.Where(log => log.ActionType == action && log.EntityType == type);
            if (start.HasValue && end.HasValue && end.Value > DateTime.MinValue)
            {
                query = query.Where(log => log.DateTime > start.Value && log.DateTime < end.Value);
            }
            return query.GroupBy(log=>log.EntityId).Count();
        }

        public IEnumerable<UserOnEntityLog> UserArtistWatchLog(string Id)
        {
            return ((PaskolDbContext)_uow.context).Database.SqlQuery<UserOnEntityLog>
                (string.Format(@"select top 20  u.UserName AS EntityName, sum(value) AS Value 
                                 from ActionOnLogDatas l join AspNetUsers u on u.Id = l.EntityId
                                 where l.userid = '{0}' AND l.EntityType = {1}
                                 group by u.UserName
                                 order by Value" , Id, (int)EntityType.Artist))
                    .ToList();
        }

        public IEnumerable<UserOnEntityLog> UserWatchTagsLogs(string id)
        {
            return ((PaskolDbContext)_uow.context).Database.SqlQuery<UserOnEntityLog>
               (string.Format(@"select top 20 t.Name AS EntityName, sum(value) AS Value 
                                 from ActionOnLogDatas l join Tags t on t.ID = l.EntityId
                                 where l.userid = '{0}' AND l.EntityType = {1}
                                 group by t.Name 
                                 order by Value", id, (int)EntityType.Tag))
                   .ToList();
        }
    }
}
