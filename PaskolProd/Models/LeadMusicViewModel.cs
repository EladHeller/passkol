using Model.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaskolProd.Models
{
    public class LeadMusicViewModel
    {
        public List<LeadMusicLog> LeadMusicLogs { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public MusicReportType ActionType { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalResults { get; set; }
    }
}