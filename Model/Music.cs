using Infrastructure.Domain;
using Model.Logs;
using Model.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum MusicActiveStatus
    {
        [Display(Name = "פעיל")]
        Public,
        [Display(Name = "נערך")]
        Edited,
        [Display(Name = "חדש")]
        New,
        [Display(Name = "עריכה ממתינה לאישור")]
        WaitingForConfirm,
        [Display(Name = "חסום")]
        Blocked
    }

    public enum SearchSortType
    {
        Relevant =1,
        LengthAsc = 2,
        LengthDesc = 3
    }

    public enum MusicFieldsSort
    {
        ArtistName,
        MusicName,
        CreateDate,
        UpdateDate,
        None
    }
    public class Music : IAggregateRoot
    {
        public Music()
        {

        }
        public Music(Music music)
        {
            this.Artist = music.Artist;
            this.ArtistID = music.ArtistID;
            this.BPM = music.BPM;
            this.Comment = music.Comment;
            this.CostLevel = music.CostLevel;
            this.CreateDate = music.CreateDate;
            this.EnglishName = music.EnglishName;
            this.Exceptions = music.Exceptions;
            this.HebrewName = music.HebrewName;
            this.ID = music.ID;
            this.MusicComposer = music.MusicComposer;
            this.MusicLength = music.MusicLength;
            this.MusicPerformer = music.MusicPerformer;
            this.MusicWriter = music.MusicWriter;
            this.SongText = music.SongText;
            this.Status = music.Status;
            this.SourceMusicId = music.SourceMusicId;
            this.UpdateDate = music.UpdateDate;

            this.SourceMusic = music.SourceMusic;
            this.Tags = music.Tags;
            this.Copyrights = music.Copyrights;
            this.MP3File = music.MP3File;
            this.WAVFile = music.WAVFile;
            this.MP3FileID = music.MP3FileID;
            this.WAVFileID = music.WAVFileID;
        }

        [Key]
        public int ID { get; set; }
        [Display(Name = "סטטוס")]
        public MusicActiveStatus Status { get; set; }
        public int? SourceMusicId { get; set; }
        public virtual Music SourceMusic { get; set; }
        [Display(Name ="שם בעברית")]
        [Required(AllowEmptyStrings =false,ErrorMessage ="שדה חובה")]
        public string HebrewName { get; set; }
        [Display(Name ="שם באנגלית")]
        public string EnglishName { get; set; }
        public string ArtistID { get; set; }
        public virtual Artist Artist { get; set; }
        public virtual File MP3File { get; set; }
        public string MP3FileID { get; set; }
        public virtual File WAVFile { get; set; }
        public string WAVFileID { get; set; }
        [Display(Name ="תאריך יצירה")]
        public DateTime CreateDate { get; set; }
        [Display(Name ="תאריך עדכון")]
        public DateTime? UpdateDate { get; set; }
        [Required(ErrorMessage ="שדה חובה")]
        [Display(Name ="אורך יצירה בדקות")]
        public TimeSpan MusicLength { get; set; }
        [Display(Name ="מילים")]
        public string SongText { get; set; }
        [Display(Name ="BPM")]
        public int BPM { get; set; }
        [Display(Name ="כותב")]
        [Required(AllowEmptyStrings =false,ErrorMessage ="שדה חובה")]
        public string MusicWriter { get; set; }
        [Display(Name = "מלחין")]
        [Required(AllowEmptyStrings =false,ErrorMessage ="שדה חובה")]
        public string MusicComposer { get; set; }
        [Display(Name = "מבצע")]
        [Required(AllowEmptyStrings =false,ErrorMessage ="שדה חובה")]
        public string MusicPerformer { get; set; }
        public virtual List<Copyright> Copyrights { get; set; }
        [Display(Name = "החרגות")]
        public string Exceptions { get; set; }
        public virtual List<Tag> Tags { get; set; }
        [Display(Name = "רמת מחיר")]
        public PriceLevel CostLevel { get; set; }
        [Display(Name = "הערת מנהל")]
        public string Comment { get; set; }
        public virtual NCDataUpload NCDataUpload { get; set; }
        public virtual ICollection<PaskolUser> UsersFavouriteMusic { get; set; }
    }
}
