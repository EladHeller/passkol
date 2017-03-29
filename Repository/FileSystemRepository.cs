using Repository.Interfaces;
using System.IO;
using System.Drawing;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Model;
using System;

namespace Repository
{
    public class FileSystemRepository : IFileSystemRepository
    {
        public string BasePath { get; set; }
        private const string FileNameFormat = "{0}_V{1}{2}";
        private const string USER_CONV = "User-";

        public FileSystemRepository(string basePath)
        {
            BasePath = basePath;
            ValidateDirectory(BasePath);
        }

        private void ValidateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public string GetRelativeFilePath(Model.FileType type, string UserId, string EntityId, string suffix, int version)
        {
            string directory = Path.Combine(type.ToString(), USER_CONV + UserId);
            return Path.Combine(directory, string.Format(FileNameFormat, EntityId, version, suffix));
        }

        public Model.File GetFile(Model.File req)
        {
            string path = Path.Combine(BasePath, GetRelativeFilePath
                (req.FileType, req.UserId, req.FileId, req.suffix, req.version));

            Model.File file = new Model.File()
            {
                FileId = Path.GetFileName(path),
                FileType = req.FileType,
                Content = System.IO.File.ReadAllBytes(path)
            };

            return file;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ft"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public IEnumerable<string> GetFilesNameForType(FileType ft, string UserId)
        {
            string path = Path.Combine(BasePath,ft.ToString(), USER_CONV + UserId);

            if (Directory.Exists(path))
            {
                return Directory.GetFiles(path).Select(d => Path.GetFileName(d)).ToArray();
            }
            else
            {
                throw new System.Exception("לא הצלחנו למצוא את הנתיב");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string StoreFile(Model.File file)
        {
            string relativePath = GetRelativeFilePath
                (file.FileType, file.UserId, file.FileId, file.suffix, file.version);
            string path = Path.Combine(BasePath, relativePath);
            ValidateDirectory(Path.GetDirectoryName(path));
            System.IO.File.WriteAllBytes(path, file.Content);

            return relativePath;
        }

        public string StoreImage(Model.File file, HttpPostedFileBase Httpfile)
        {
            string relativePath = GetRelativeFilePath(file.FileType, file.UserId, file.FileId, file.suffix, file.version);
            string path = Path.Combine(BasePath, relativePath);
            ValidateDirectory(Path.GetDirectoryName(path));
            Httpfile.SaveAs(path);

            return string.Format(FileNameFormat, file.FileId, 1, file.suffix);
        }

        public string GetFilePath(Model.File req)
        {
            string path = Path.Combine(BasePath, GetRelativeFilePath
                (req.FileType, req.UserId, req.FileId, req.suffix, req.version));
            return path;
        }
    }
}
