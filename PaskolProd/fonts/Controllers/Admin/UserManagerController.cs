using PaskolProd.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using Model.Users;
using PaskolProd.Models;
using PagedList;
using PaskolProd.Models.Helper;

namespace PaskolProd.Controllers.Admin
{
    public class UserManagerController : Controller
    {
        /// <summary>
        /// The User Manager Stored at the HttpContext
        /// </summary>
        private PaskolUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<PaskolUserManager>();
            }
        }

        /// <summary>
        /// The Role Manager Stored at the HttpContext
        /// </summary>
        private PaskolRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<PaskolRoleManager>();
            }
        }

        // GET: UserManager
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page, string ddlRole)
        {
            List<string> roles = new List<string>();

            //Set SearchByRole
            foreach (string name in Enum.GetNames(typeof(UserType)))
            {
                if (!name.Contains("Admin"))
                {
                    roles.Add(name);
                }
            }

            ViewBag.Roles = roles;

            // Set sorting params
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var Users = from u in UserManager.Users
                           select u;

            // Where handle
            if (!String.IsNullOrEmpty(searchString))
            {
                Users = Users.Where(s => s.UserName.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(ddlRole))
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
                case "date_desc":
                    Users = Users.OrderByDescending(s => s.RegisteredDate);
                    break;
                default:
                    Users = Users.OrderBy(s => s.UserName);
                    break;
            }

            int pageSize = 25;
            int pageNumber = (page ?? 1);
            return View(Users.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult BlockActive(string ID)
        {
            PaskolUser user = UserManager.FindById(ID);

            if (user != null)
            {
                if (user.Status == Status.Blocked || user.Status == Status.WithoutStatus)
                {
                    user.Status = Status.Active;
                }
                else if (user.Status == Status.Active)
                {
                    user.Status = Status.Blocked;
                }

                UserManager.Update(user);
            }

            return RedirectToAction("Index");
        }
        public async Task<ActionResult> UserDetails(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return RedirectToAction("Index");
            }

            PaskolUser user = await UserManager.FindByIdAsync(Id);

            if (user != null)
            {
                if (user.UserType == UserType.Artist)
                {
                    return View("ArtistDetails", user.CreateArtistViewFrom());
                }
                else if (user.UserType == UserType.Customer)
                {
                    return View("CustomerDetails", user.CreateCustomerViewFrom());
                }
                else if (user.UserType == UserType.ArtistAgent)
                {
                    return View("ArtistAgentDetails", user.CreateCustomerViewFrom());
                }
            }
            
            return View("Error", new string[] { "User Not Found" });
        }

        public async Task<ActionResult> Edit(string id)
        {
            PaskolUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> EditArtist(ArtistDetailsView EditedArtist)
        {
            PaskolUser user = await UserManager.FindByIdAsync(EditedArtist.UserID);

            if (user != null)
            {

                // Save artist details
                EditedArtist.UpdatePaskolUserFrom(user);

                IdentityResult validPass = null;

                if (EditedArtist.Password != null)
                {
                    validPass = await UserManager.PasswordValidator.ValidateAsync(EditedArtist.Password);

                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = UserManager.PasswordHasher.HashPassword(EditedArtist.Password);


                        IdentityResult result = await UserManager.UpdateAsync(user);

                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            AddErrorsFromResult(result);
                        }
                    }
                    else
                    {
                        AddErrorsFromResult(validPass);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found"); 
            }

            return View("UserDetails",user);
        }

        public async Task<ActionResult> EditCustomer(CustomerDetailsView EditedCustomer)
        {
            PaskolUser user = await UserManager.FindByIdAsync(EditedCustomer.UserID);

            if (user != null)
            {
                // Save artist details
                EditedCustomer.UpdatePaskolUserFrom(user);

                IdentityResult validPass = null;

                if (EditedCustomer.Password != null)
                {
                    validPass = await UserManager.PasswordValidator.ValidateAsync(EditedCustomer.Password);

                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = UserManager.PasswordHasher.HashPassword(EditedCustomer.Password);


                        IdentityResult result = await UserManager.UpdateAsync(user);

                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            AddErrorsFromResult(result);
                        }
                    }
                    else
                    {
                        AddErrorsFromResult(validPass);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }

            return View("UserDetails", user);
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}
