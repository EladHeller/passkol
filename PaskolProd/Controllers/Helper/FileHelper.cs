using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaskolProd.Controllers.Helper
{
    public class FileHelper
    {
        public static Model.File ConvertPostedFileToFile
            (HttpPostedFileBase postedFile, int version, string userId, string fileId)
        {
            Model.File resFile = null;
            if (postedFile != null)
            {
                resFile = new Model.File()
                {
                    FileType = FileType.Music,
                    suffix = System.IO.Path.GetExtension(postedFile.FileName),
                    UserId = userId,
                    version = version,
                };
                if (!string.IsNullOrEmpty(fileId))
                {
                    resFile.FileId = fileId;
                }
                var wavLength = postedFile.InputStream.Length;
                resFile.Content = new byte[wavLength];
                postedFile.InputStream.Read(resFile.Content, 0, (int)wavLength);
            }
            return resFile;
        }
    }
}