using PaskolProd.Models;
using Services;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using PaskolProd.Authentication;

namespace PaskolProd.Areas.Admin.Controllers
{
    public class DocumentsController : UserBaseController
    {
        private FileSystemService FSservice = ServiceLocator.GetService<FileSystemService>();

        // GET: Documents
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult GetDocumnets(string Id)
        {
            var user = UserManager.Users.SingleOrDefault(u => u.Id == Id);

            if (user != null)
            {
                UserDocumentsViewModel model = new UserDocumentsViewModel();
                model.Documents = new Dictionary<string, string>();
                var res = FSservice.GetAllFilesName(FileType.PDF, Id);
                if (res.Success)
                {
                    // Get only pdf files
                    model.Documents = res.Entities.Where(d => d.Contains(".pdf")).Select((p) => new {
                        key = p,
                        value = p
                    })
                    .ToDictionary(k => k.key, v => v.value);
                }

                model.User = user;
                // Save url to geet back to after action ends
                ViewBag.ReturnURL = Request.UrlReferrer;
                return View(model);
            }

            throw new HttpException(404, "משתמש לא נמצא");
        }
    }
}