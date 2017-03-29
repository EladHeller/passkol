using Model.Logs;
using Model.Purchase;
using Model.Users;
using System.Collections.Generic;

namespace PaskolProd.Models
{
    public class UserReportViewModel
    {
        public PaskolUser User { get; set; }
        public IEnumerable<UserMusicLog> UserMusicLogs { get; set; }
        public IEnumerable<UserOnEntityLog> UserWatchArtistLogs { get; set; }
        public IEnumerable<UserOnEntityLog> UserWatchTagsLogs { get; set; }
        public IEnumerable<Purchase> Purchases { get; set; }
    }
}