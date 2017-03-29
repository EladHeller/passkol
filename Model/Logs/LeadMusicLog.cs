using Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Logs
{
    public class LeadMusicLog : IAggregateRoot
    {
        public int MusicID { get; set; }
        public string ArtistName { get; set; }
        public string HebrewName { get; set; }
        public int Count { get; set; }
        public double AvgCost { get; set; }
        public double SumCosts { get; set; }
        public double ArtistEarn { get; set; }
        public double PasskolEarn { get; set; }
    }
}
