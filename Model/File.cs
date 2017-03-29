using Infrastructure.Domain;
using Model.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Model
{
    public class File : IAggregateRoot
    {
        private string _suffix;
        private int _version;
        private string _fileId;
        private string _filePath;
        private const string FileNameFormat = "{0}_V{1}{2}";
        private const string USER_CONV = "User-";

        public string FileId
        {
            get
            {
                // remove extntion
                string final = Path.GetFileNameWithoutExtension(this._fileId);
                //remove version if has
                final = final.IndexOf("_V") > 0 ? final.Substring(0, final.IndexOf("_V")) : final;
                return final;
            }
            set
            {
                this._fileId = value;
            }
        }

        //[StringLength(255)]
        //public string FileName { get; set; }
        [StringLength(100)]
        public string ContentType
        {
            get
            {
                return string.IsNullOrEmpty(_fileId) ? "" : MimeMapping.GetMimeMapping(this._fileId);
            }
        }
        [NotMapped]
        public byte[] Content { get; set; }
        public FileType FileType { get; set; }
        public string UserId { get; set; }
        public string suffix
        {
            get
            {
                if (string.IsNullOrEmpty(this._suffix))
                {
                    return Path.GetExtension(this._fileId);
                }

                return this._suffix;
            }
            set
            {
                this._suffix = value;
            }
        }

        public string RelativePath
        {
            get
            {
                if (string.IsNullOrEmpty(_filePath))
                {
                    _filePath = Path.Combine(this.FileType.ToString(), USER_CONV + this.UserId, string.Format(FileNameFormat, FileId, version, suffix));
                }

                return _filePath;
            }
        }

        public int version
        {
            get
            {
                if (this._version == 0)
                {
                    // if we have version in file
                    if (this._fileId.IndexOf("_V") > 0)
                    {
                        string strRes = _fileId.Substring(_fileId.IndexOf("_V") + 2, _fileId.IndexOf(".") - _fileId.IndexOf("_V") - 2);
                        int nRes;

                        if (int.TryParse(strRes, out nRes))
                        {
                            return nRes;
                        }
                    }
                }

                return this._version;
            }
            set
            {
                this._version = value;
            }
        }

        public File()
        {
            FileId = Guid.NewGuid().ToString();
        }
    }
}
