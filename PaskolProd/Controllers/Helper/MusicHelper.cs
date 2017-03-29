using Model;
using PaskolProd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaskolProd.Controllers.Helper
{
    public class MusicFilesFromHttpFiles
    {
        public Model.File mp3File { get; set; }
        public Model.File wavFile { get; set; }
    }

    public class MusicHelper
    {
        public MusicFilesFromHttpFiles SetMusicBeforeUpDateForArtist(
            Music music, 
            MusicSearchModel musicFromClient,
            HttpPostedFileBase WAVUpload,
            HttpPostedFileBase MP3Upload)
        {
            ConvertMusicFromSearchMusicmodel(music, musicFromClient);
            MusicFilesFromHttpFiles filesToReturn = new MusicFilesFromHttpFiles();

            filesToReturn.mp3File = FileHelper.
                                ConvertPostedFileToFile(MP3Upload, music.MP3File?.version + 1 ?? 0, music.ArtistID, music.MP3FileID);
            filesToReturn.wavFile = FileHelper.
                ConvertPostedFileToFile(WAVUpload, music.WAVFile?.version + 1 ?? 0, music.ArtistID, music.WAVFileID);
            
            return filesToReturn;
        }

        private void ConvertMusicFromSearchMusicmodel(Music music, MusicSearchModel modelMusic)
        {
            music.BPM = modelMusic.BPM;
            music.EnglishName = modelMusic.EnglishName;
            music.Exceptions = modelMusic.Exceptions;
            music.HebrewName = modelMusic.HebrewName;
            music.MusicComposer = modelMusic.MusicComposer;
            music.MusicPerformer = modelMusic.MusicPerformer;
            music.MusicWriter = modelMusic.MusicWriter;
            music.SongText = modelMusic.SongText;
            music.Copyrights = modelMusic.Copyrights.Select(c => new Copyright(c)).ToList();
            music.MusicLength = new TimeSpan(0, modelMusic.Minutes, modelMusic.Seconds);
        }

        public MusicFilesFromHttpFiles SetMusicBeforeUpDate(MusicViewModelEntry music, HttpPostedFileBase WAVUpload,
            HttpPostedFileBase MP3Upload)
        {
            MusicFilesFromHttpFiles filesToReturn = new MusicFilesFromHttpFiles();

            filesToReturn.mp3File = FileHelper.
                                ConvertPostedFileToFile(MP3Upload, music.MP3File?.version + 1 ?? 0, music.ArtistID, music.MP3FileID);
            filesToReturn.wavFile = FileHelper.
                ConvertPostedFileToFile(WAVUpload, music.WAVFile?.version + 1 ?? 0, music.ArtistID, music.WAVFileID);
            
            music.Tags = music.TagViews?
                .Where(tg => tg.IsNotDeleted)
                .Select(tg => {
                    var tag = new Tag(tg);
                    tag.ID = tg.ID;
                    return tag;
                })
                .ToList();

            music.Copyrights = music.Copyrights?.Where(copy => !copy.IsDeleted).ToList();
            music.MusicLength = new TimeSpan(0, music.Minutes, music.Seconds);
            return filesToReturn;
        }
    }
}