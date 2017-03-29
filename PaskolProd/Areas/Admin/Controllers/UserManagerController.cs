using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using PaskolProd.Models;
using PagedList;
using PaskolProd.Models.Helper;
using System.Data.Entity;
using Services;
using Model.Users;
using Model.Helper;
using Infrastructure.Configuration;
using Services.PDFService;
using Model.Filters;

namespace PaskolProd.Areas.Admin.Controllers
{
    public class UserManagerController : UserBaseController
    {
        private FileSystemService FSservice;
        private PDFServiceSoapClient _pdfService;

        public UserManagerController()
        {
            FSservice = ServiceLocator.GetService<FileSystemService>();
            _pdfService = ServiceLocator.GetService<PDFServiceSoapClient>();
        }

        [Authorize(Roles = "SystemAdmin")]
        public ActionResult CreateArtist(string urlReferrer)
        {
            // Save url to geet back to after action ends
            ViewBag.ReturnURL = urlReferrer ?? Request.UrlReferrer?.ToString();
            return View();
        }

        [Authorize(Roles = "SystemAdmin")]
        public ActionResult CreateCustomer(string urlReferrer)
        {
            // Save url to geet back to after action ends
            ViewBag.ReturnURL = urlReferrer ?? Request.UrlReferrer?.ToString();
            return View();
        }

        [Authorize(Roles = "SystemAdmin")]
        public ActionResult CreateArtistAgent(string urlReferrer)
        {
            // Save url to geet back to after action ends
            ViewBag.ReturnURL = urlReferrer ?? Request.UrlReferrer?.ToString();
            return View();
        }
        
        [HttpPost]
        [Authorize(Roles = "SystemAdmin")]
        public async Task<ActionResult> CreateArtist(ArtistCreateView model, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                PaskolUser user = model.UserFromArtistEditView(null);
                user.Status = UserStatus.Active;

                // Save photo if have
                var PhotoRes = FSservice.StorePhoto(upload, user);

                if (PhotoRes.Success)
                {
                    user.Artist.PhotoID = PhotoRes.PhotoId;

                    // Call userbase CreateUserAsync
                    IdentityResult res = await CreateUserAsync(user, model.Password,
                        new string[] { UserType.Artist.ToString() });

                    if (res.Succeeded)
                    {
                        // Create PermissionAgreement
                        _pdfService.ArtistPermissionAgreement
                            (DateTime.Now, user.UserName, user.Email, user.Id, WebConf.FSBaseRoute);

                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError("PictureId", PhotoRes.Message);
                }
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "SystemAdmin")]
        public async Task<ActionResult> CreateCustomer(CustomerEditView model)
        {
            if (ModelState.IsValid)
            {
                PaskolUser user = model.UserFromCustomerDetails(null);
                user.Status = UserStatus.Active;

                IdentityResult res = await CreateUserAsync(user, model.Password,
                    new string[] { UserType.Customer.ToString() });

                if (res.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            return View();
        }
        
        [HttpPost]
        [Authorize(Roles = "SystemAdmin")]
        public async Task<ActionResult> CreateArtistAgent(ArtistAgentEditView model)
        {
            if (ModelState.IsValid)
            {
                // For now artist and customer have the same properties
                PaskolUser user = model.UserFromArtistAgentDetails(null);
                user.Status = UserStatus.Active;

                IdentityResult res = await CreateUserAsync(user, model.Password,
                    new string[] { UserType.ArtistAgent.ToString() });

                if (res.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            return View();
        }

        // GET: UserManager
        [Authorize(Roles = "SystemAdmin,AdminProfile")]
        public ActionResult Index(string sortOrder, string searchName, string searchContactManName, 
            string currentFilter, int? page, string ddlRole)
        {
            const string SELECTAll = "-1";
            var Roles = Enum.GetValues(typeof(UserType))
                        .Cast<UserType>()
                        .Where(t => t == UserType.Artist || t == UserType.ArtistAgent || t == UserType.Customer)
                        .Select(e => new EnumKeyVal()
                        {
                            Key = e.ToString(),
                            Value = e.ToDescription()
                        })
                        .ToList();
           
            ViewBag.Roles = Roles;
            ViewBag.Roles.Add(new EnumKeyVal()
            {
                Key = SELECTAll,
                Value = "הכל"
            });
           
            // Set params
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "Date" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.searchName = searchName;
            ViewBag.searchContactManName = searchContactManName;
            ViewBag.SelectedRole = ddlRole ?? SELECTAll;

            var Users = from u in UserManager.Users
                        where u.UserType != UserType.Admin
                        select u;

            // Where handle 
            if (!string.IsNullOrEmpty(searchName))
            {
                Users = Users.Where(s => s.UserName.StartsWith(searchName));
            }

            if (!string.IsNullOrEmpty(searchContactManName))
            {
                Users = Users
                    .Where(u => u.UserType == UserType.Artist)
                    .Where(s => s.Artist.ContactManName.StartsWith(searchContactManName));
            }

            if (User.IsInRole("AdminProfile"))
            {
                Users = Users.Where(u => u.UserType == UserType.Artist);
            }
            if (!string.IsNullOrEmpty(ddlRole) && ddlRole != "-1")
            {
                Users = Users.Where(s => s.UserType.ToString() == ddlRole);
            }

            // Sort by param
            switch (sortOrder)
            {
                case "name_desc":
                    Users = Users.OrderByDescending(s => s.UserName);
                    break;
                case "Date":
                    Users = Users.OrderBy(s => s.RegisteredDate);
                    break;
                case "Name":
                    Users = Users.OrderBy(s => s.UserName);
                    break;
                default:
                    Users = Users.OrderByDescending(s => s.RegisteredDate);
                    break;
            }

            int pageSize = 25;
            int pageNumber = (page ?? 1);
            return View(Users.ToPagedList(pageNumber, pageSize));
        }
        
        [HttpGet]
        [Authorize(Roles = "SystemAdmin")]
        public ActionResult GetBlockActive(string Id)
        {
            PaskolUser user = UserManager.Users.Single(u => u.Id == Id);

            ViewBag.HasUser = false;

            if (user != null)
            {
                ViewBag.HasUser = true;
                ViewBag.UserId = user.Id;
                ViewBag.Name = user.UserName;
                ViewBag.Email = user.Email;
                ViewBag.Type = user.UserType.ToDescription();
                ViewBag.MgrNote = user.Note;
                ViewBag.Status = user.Status;
            }
            
            return View();
        }

        [Authorize(Roles = "SystemAdmin,AdminProfile")]
        public async Task<ActionResult> EditArtist(string Id)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                PaskolUser user = await this.UserManager.Users
                    .SingleOrDefaultAsync(c => c.Id == Id);

                if (user != null)
                {
                    return View(new ArtistEditView(user));
                }
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "SystemAdmin")]
        public async Task<ActionResult> EditCustomer(string Id)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                PaskolUser user = await UserManager.FindByIdAsync(Id);

                if (user != null)
                {
                    return View(new CustomerEditView(user));
                }
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "SystemAdmin")]
        public async Task<ActionResult> EditArtistAgent(string Id)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                PaskolUser user = await UserManager.FindByIdAsync(Id);

                if (user != null)
                {
                    return View(new ArtistAgentEditView(user));
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "SystemAdmin,AdminProfile")]
        public async Task<ActionResult> EditArtist(ArtistEditView EditedArtist, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                PaskolUser user = await UserManager
                    .FindByIdAsync(EditedArtist.UserID);

                if (user != null)
                {
                    var res = FSservice.StorePhoto(upload, user, user.Artist.PhotoID);

                    if (res.Success)
                    {
                        // Save artist details
                        EditedArtist.UserFromArtistEditView(user);

                        user.Artist.PhotoID = res.PhotoId;
                        if (await UpdateUserAsync(user, EditedArtist.Password))
                        {
                            return RedirectToAction("index");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("PictureId", res.Message);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "משתמש לא נמצא");
                }
            }

            return View(EditedArtist);
        }

        [HttpPost]
        [Authorize(Roles = "SystemAdmin")]
        public async Task<ActionResult> EditCustomer(CustomerEditView EditedCustomer)
        {
            if (ModelState.IsValid)
            {
                PaskolUser user = await UserManager.FindByIdAsync(EditedCustomer.UserID);

                if (user != null)
                {
                    // Save artist details
                    EditedCustomer.UserFromCustomerDetails(user);

                    if (await UpdateUserAsync(user, EditedCustomer.Password))
                    {
                        return RedirectToAction("index");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User Not Found");
                }
            }

            return View(EditedCustomer);
        }

        [HttpPost]
        [Authorize(Roles = "SystemAdmin")]
        public async Task<ActionResult> EditArtistAgent(ArtistAgentEditView EditedArtistAgent)
        {
            if (ModelState.IsValid)
            {
                PaskolUser user = await UserManager.FindByIdAsync(EditedArtistAgent.UserID);

                if (user != null)
                {
                    // Save artist details
                    EditedArtistAgent.UserFromArtistAgentDetails(user);

                    if (await UpdateUserAsync(user, EditedArtistAgent.Password))
                    {
                        return RedirectToAction("index");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User Not Found");
                }
            }

            return View(EditedArtistAgent);
        }
    }
}
