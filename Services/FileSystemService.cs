using Repository.UnitOfWork;
using Repository.Interfaces;
using Services.Messaging.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Messaging.Requests;
using Repository;
using System.IO;
using System.Web;
using System.Drawing;
using Model.Users;
using Model;

namespace Services
{
    public class FileSystemService
    {
        private IFileSystemRepository _rep;

        public FileSystemService(IFileSystemRepository rep) 
        {
            this._rep = rep;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private bool ValidateImage(HttpPostedFileBase file)
        {
            bool IsBitmap = false;
            
            try
            {
                using (var bitmap = new Bitmap(file.InputStream))
                {
                    if (file.ContentType.ToLower() == "image/jpg" ||
                        file.ContentType.ToLower() == "image/jpeg" ||
                        file.ContentType.ToLower() == "image/pjpeg" ||
                        file.ContentType.ToLower() == "image/gif" ||
                        file.ContentType.ToLower() == "image/x-png" ||
                        file.ContentType.ToLower() == "image/png")
                    {
                        IsBitmap = true;
                    }
                }
            }
            catch (Exception)
            {
                
            }

            return IsBitmap;
        }

        public EntityResponse<Model.File> Get(string path)
        {
            EntityResponse<Model.File> res = new EntityResponse<Model.File>();

            try
            {
                Model.File file = new Model.File()
                {
                    FileId = Path.GetFileName(path),
                    FileType = FileType.PDF,
                    Content = System.IO.File.ReadAllBytes(path)
                };
                res.Entity = file;
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Success = false;
                res.Message = e.Message;
            }

            return res; 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetFileNameWithoutVersion(string name)
        {
            string FileNameWithoutExtension = Path.GetFileNameWithoutExtension(name);

            return FileNameWithoutExtension.Substring(0, FileNameWithoutExtension.IndexOf("_"));
        }
        
        public EntityResponse<string> GetFilePath(Model.File req)
        {
            EntityResponse<string> res = new EntityResponse<string>();

            try
            {
                res.Entity = this._rep.GetFilePath(req);
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Message = e.Message;
                res.Success = false;
            }

            return res;
        }

        public EntityResponse<Model.File> GetFile(Model.File req)
        {
            EntityResponse<Model.File> res = new EntityResponse<Model.File>();

            try
            {
                res.Entity = this._rep.GetFile(req);
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Message = e.Message;
                res.Success = false;
            }
            
            return res;
        }

        public EntityResponse<Model.File> StoreFile(Model.File file)
        {
            var res = new EntityResponse<Model.File>();
            try
            {
                var filePath = this._rep.StoreFile(file);
                res.Entity = file;
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Message = e.ToString();
                res.Success = false;
            }
            return res;
        }

        public SavePhotoResponse StorePhoto(HttpPostedFileBase file, PaskolUser user, string OldPhotoId = null)
        {
            SavePhotoResponse res = new SavePhotoResponse();
            res.Success = true;
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    var IsBitMap = ValidateImage(file);
                    // If file is image
                    if (IsBitMap)
                    {
                        var photo = new Model.File()
                        {
                            FileId = System.IO.Path.GetFileName(file.FileName),
                            FileType = FileType.Photo,
                            version = 1,
                            UserId = user.Id
                        };
                        
                        string fi = this._rep.StoreImage(photo, file);
                        res.PhotoId = fi;
                    }
                    else
                    {
                        res.Success = false;
                        res.Message = "יש להעלות קובץ תמונה בלבד";
                    }
                }
            }
            catch (Exception e)
            {
                res.Message = e.Message;
                res.Success = false;
                throw;
            }
            
            return res;
        }
        
        public EntityAllResponse<string> GetAllFilesName(FileType ft, string UserId)
        {
            EntityAllResponse<string> res = new EntityAllResponse<string>();
            try
            {
                res.Entities = this._rep.GetFilesNameForType(ft, UserId);
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Message = e.Message;
                res.Success = false;
            }
            return res;
        }

       
    }
}
