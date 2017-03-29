using Model;
using Model.Helper;
using PaskolProd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaskolProd.Models
{
    public class MusicViewModel
    {
        public IEnumerable<MusicViewModelEntry> Musics { get; set; }
        public bool? Success { get; set; }
    }
    public class MusicViewModelEntry : Music
    {
        public List<TagViewModel> TagViews { get; set; }
        public string StatusDescription { get; set; }
        public int Seconds { get; set; }
        public int Minutes { get; set; }
        public int CopyrightsPercents { get; set; }

        public MusicViewModelEntry()
        {

        }
        public MusicViewModelEntry(Music music) : base(music)
        {
            this.CopyrightsPercents = this.Copyrights?.Sum(copy => copy.Percents) ?? 0;
            this.StatusDescription = music.Status.ToDescription();
        }
    }
}