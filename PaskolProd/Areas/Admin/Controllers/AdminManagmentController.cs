using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using PaskolProd.Models;
using PagedList;
using PaskolProd.Models.Helper;
using Model.Users;
using Model.Helper;
using Model.Filters;
using System.Web.Security;

namespace PaskolProd.Areas.Admin.Controllers
{
    [Authorize(Roles ="SystemAdmin")]
    public class AdminManagmentController : UserBaseController
    {
        [HttpGet]
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
                ViewBag.Roles = GetAdminRoles()
                        .Where(ar => RoleManager.Roles.ToList()
                            .Where(r => user.Roles.Any(ur => ur.RoleId == r.Id))
                            .Any(g => g.Name == ar.Key))
                        .Select(v => v.Value)
                        .ToList();
                ViewBag.MgrNote = user.Note;
                ViewBag.Status = user.Status;
            }

            // Save url to geet back to after action ends
            ViewBag.ReturnURL = Request.UrlReferrer;
            return View();
        }
        
        // GET: AdminManagmen
        public ActionResult Index()
        {
            List<ManagersCreateView> UsersObj = UserManager.Users
                .Where(u => u.UserType == UserType.Admin).ToList()
                .Select(user => new ManagersCreateView
                {
                    UserID = user.Id,
                    Name = user.UserName,
                    RegisteredDate = user.RegisteredDate,
                    Status = user.Status.ToDescription(),
                    Note = user.Note,
                    Roles = GetAdminRoles()
                        .Where(ar => RoleManager.Roles.ToList()
                            .Where(r => user.Roles.Any(ur => ur.RoleId == r.Id))
                            .Any(g => g.Name == ar.Key))
                        .Select(v => v.Value)
                        .ToList()
                    //RoleManager.Roles.ToList().Where(r => user.Roles.Any(ur => ur.RoleId == r.Id)).Select(r => r.Name).ToList()
                }).ToList();
            return View(UsersObj);
        }
        
        // GET: AdminManagment/Create
        public ActionResult Create()
        {
            // Check if we have some store modelstate
            if (TempData["ViewData"] != null)
            {
                ViewData = (ViewDataDictionary)TempData["ViewData"];
            }

            ViewBag.Roles = GetAdminRoles();
            // Save url to geet back to after action ends
            ViewBag.ReturnURL = Request.UrlReferrer;
            return View();
        }

        // POST: AdminManagment/Create
        [HttpPost]
        public async Task<ActionResult> Create(ManagersCreateView model)
        {
            if (ModelState.IsValid)
            {
                PaskolUser user = model.UserFromUserDetails(null);
                user.Status = UserStatus.Active;
                user.UserType = UserType.Admin;

                IdentityResult res = await CreateUserAsync(user, model.Password, model.Roles);

                if (res.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            // Save model state in order to represent it under Redirection
            TempData["ViewData"] = ViewData;

            return RedirectToAction("Create");
        }

        // GET: AdminManagment/Edit/5
        public ActionResult Edit(string id)
        {
            if (id != null)
            {
                // Check if we have some store modelstate
                if (TempData["ViewData"] != null)
                {
                    ViewData = (ViewDataDictionary)TempData["ViewData"];
                }

                ViewBag.Roles = GetAdminRoles();
                PaskolUser user = UserManager.Users.Where(u => u.Id == id).FirstOrDefault();

                // Save url to geet back to after action ends
                ViewBag.ReturnURL = Request.UrlReferrer;
                var mev = new ManagersEditView(user);
                mev.Roles = RoleManager.Roles.Where(r => mev.Roles.Any(ur => ur == r.Id)).Select(r => r.Name).ToList();
                return View(mev);
            }

            return RedirectToAction("Index");
        }

        // POST: AdminManagment/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(ManagersEditView model, string ReturnURL)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    PaskolUser user = await UserManager.FindByIdAsync(model.UserID);

                    if (user == null)
                    {
                        ModelState.AddModelError("", "משתמש לא נמצא");
                    }
                    else
                    {
                        
                        user.UserName = model.Name;
                        user.Email = model.Email;
                        user.Note = model.Note;

                        // User cant change permission to his self
                        if (user.Id != User.Identity.GetUserId())
                        {
                            // Before change role names
                            IEnumerable<string> BeforeChange = user.Roles
                                .Select(r => RoleManager.Roles.SingleOrDefault(rn => rn.Id == r.RoleId).Name);

                            model.Roles = model.Roles ?? new List<string>();
                            // Ids to add
                            IEnumerable<string> RoleIdsToAdd = model.Roles.Except(BeforeChange);
                            // Ids to delete
                            IEnumerable<string> RoleIdsToDelete = BeforeChange.Except(model.Roles);

                            IdentityResult result = null;

                            //Delete and add roles
                            foreach (string roleName in RoleIdsToAdd.ToList())
                            {
                                result = await UserManager.AddToRoleAsync(user.Id, roleName);
                                if (!result.Succeeded)
                                {
                                    return View("Error", result.Errors);
                                }
                            }

                            foreach (string roleName in RoleIdsToDelete.ToList())
                            {
                                result = await UserManager.RemoveFromRoleAsync(user.Id, roleName);
                                if (!result.Succeeded)
                                {
                                    return View("Error", result.Errors);
                                }
                            }
                        }

                        if (await UpdateUserAsync(user, model.Password))
                        {
                            return RedirectToAction("index");
                        }
                    }
                }
                catch(Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }

            TempData["ViewData"] = ViewData;

            return RedirectToAction("Edit", new { id = model.UserID });
        }
    }
}
