using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Repository.Interfaces
{
    public interface IFileSystemRepository
    {
        string BasePath { get; set; }
        Model.File GetFile(Model.File file);
        string StoreFile(Model.File file);
        string StoreImage(Model.File file, HttpPostedFileBase Httpfile);
        IEnumerable<string> GetFilesNameForType(FileType ft, string UserId);
        string GetFilePath(File req);
    }
}
