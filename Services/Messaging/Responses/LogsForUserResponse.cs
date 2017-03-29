using Model.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Messaging.Responses
{
    public class LogsForUserResponse : ResponseBase
    {
        public IEnumerable<UserMusicLog> UserMusicLogs { get; set; }
        public IEnumerable<UserOnEntityLog> UserWatchArtistLogs { get; set; }
        public IEnumerable<UserOnEntityLog> UserWatchTagsLogs { get; set; }
    }
}
