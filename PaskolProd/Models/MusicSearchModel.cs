using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaskolProd.Models
{
    public class MusicSearchResult
    {
        public IEnumerable<MusicSearchModel> Musics { get; set; }
        public int TotalResults { get; set; }
        public int TotalPages { get; set; }
    }

    public class MusicSearchModel
    {
        public int? ID { get; set; }
        public string HebrewName { get; set; }
        public string EnglishName { get; set; }
        public string ArtistID { get; set; }
        public string ArtistName { get; set; }
        public string ArtistPageUrl { get; set; }
        public File MP3File { get; set; }
        public string MP3FileID { get; set; }
        public File WAVFile { get; set; }
        public string WAVFileID { get; set; }
        public string MusicLength { get; set; }
        public string SongText { get; set; }
        public int BPM { get; set; }
        public string MusicWriter { get; set; }
        public string MusicComposer { get; set; }
        public string MusicPerformer { get; set; }
        public string Exceptions { get; set; }
        public bool IsFavourite { get; set; }
        public IEnumerable<TagViewModel> Tags { get; set; }

        public IEnumerable<CopyrightViewModel> Copyrights { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsConfirmed { get; set; }
        public int Seconds { get; set; }
        public int Minutes { get; set; }

        public MusicSearchModel()
        {

        }

        public MusicSearchModel(Music music, bool isFavourite, bool isMySongsModel = false)
        {
            HebrewName = music.HebrewName;
            EnglishName = music.EnglishName;
            ID = music.ID;
            ArtistID = music.ArtistID;
            ArtistName = music.Artist.User.UserName;
            ArtistPageUrl = music.Artist.PageUrl;
            BPM = music.BPM;
            Exceptions = music.Exceptions;

            MP3File = music.MP3File;
            MP3FileID = music.MP3FileID;
            WAVFile = music.WAVFile;
            WAVFileID = music.WAVFileID;

            MusicComposer = music.MusicComposer;
            MusicPerformer = music.MusicPerformer;
            MusicWriter = music.MusicWriter;
            MusicLength = music.MusicLength.ToString(@"mm\:ss");
            SongText = music.SongText;
            Tags = music.Tags.Select(tag => new TagViewModel(tag, tag.ID, false, false, false, false));
            if (isMySongsModel)
            {
                Copyrights = music.Copyrights.Select(c => new CopyrightViewModel(c));
                CreateDate = music.CreateDate;
                UpdateDate = music.UpdateDate;
                IsConfirmed = music.Status == MusicActiveStatus.Public;
                Seconds = music.MusicLength.Seconds;
                Minutes = music.MusicLength.Minutes;
            }
            IsFavourite = isFavourite;
        }
    }
}