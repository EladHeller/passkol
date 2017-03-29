using Model;
using Model.Filters;
using Model.PriceModel;
using PaskolProd.Models;
using Services;
using Services.Messaging.Responses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PaskolProd.Areas.Admin.Controllers
{
    [Authorize(Roles = "SystemAdmin")]
    public class PermissionCategoryController : Controller
    {
        private PermissionCategoryService service;

        public PermissionCategoryController()
        {
            service = ServiceLocator.GetService<PermissionCategoryService>();
        }
             
        // GET: Permissions
        public ActionResult Index(bool sortByName = false, bool sortDesc=false)
        {
            var res = service.GetAll();
            if (!res.Success)
            {
                return Json(res.Message, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (res.Entities != null && res.Entities.Any())
                {
                    if (sortDesc)
                    {
                        if (sortByName)
                        {
                            res.Entities = res.Entities.OrderByDescending(x => x.Name);   
                        }
                        else
                        {
                            res.Entities = res.Entities.OrderByDescending(x => x.ViewOrder);
                        }
                    }
                    else
                    {
                        if (sortByName)
                        {
                            res.Entities = res.Entities.OrderBy(x => x.Name);
                        }
                        else
                        {
                            res.Entities = res.Entities.OrderBy(x => x.ViewOrder);
                        }
                    }
                }

                ViewBag.SortByName = sortByName;
                ViewBag.SortDesc = sortDesc;
                return View(res.Entities);
            }
        }

        // GET: Permissions/Create
        public ActionResult Create(bool sortByName = false, bool sortDesc = false)
        {
            PermissionsCategory perCat = new PermissionsCategory();
            perCat.ViewOrder = 10;
            perCat.IsActive = true;
            ViewBag.SortByName = sortByName;
            ViewBag.SortDesc = sortDesc;

            return View(perCat);
        }

        // POST: Permissions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,IsActive,ViewOrder")] PermissionsCategory permissionsCategory, bool sortByName = false, bool sortDesc = false)
        {
            if (ModelState.IsValid)
            {
                permissionsCategory.ID = Guid.NewGuid();
                permissionsCategory.PermissionProperties =
                    PermissionsCategory.CreateCostLevelProperties(permissionsCategory.ID);
                var res = service.Add(permissionsCategory);
                if (!res.Success)
                {
                    return Json(res.Message, JsonRequestBehavior.AllowGet);
                }

                return RedirectToAction("Index");
            }

            return View(permissionsCategory);
        }

        // GET: Permissions/Edit/5
        public ActionResult Edit(Guid? id, bool sortByName = false, bool sortDesc = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var res = service.GetByID(id.Value);
            PermissionsCategory permissionsCategory = res.Entity;

            if (!res.Success)
            {
                return Json(res.Message);
            }

            if (permissionsCategory == null)
            {
                return HttpNotFound();
            }

            return View(permissionsCategory);
        }

        // POST: Permissions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,IsActive,ViewOrder")] PermissionsCategory permissionsCategory, bool sortByName = false, bool sortDesc = false)
        {
            if (ModelState.IsValid)
            {
                var res = service.Update(permissionsCategory);
                if (res.Success)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", res.Message);
                }
            }

            return View(permissionsCategory);
        }

        // GET: Permissions/Delete/5
        public ActionResult Delete(Guid? id, bool sortByName = false, bool sortDesc = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var res = service.GetByID(id.Value);
            PermissionsCategory permissionsCategory = res.Entity;

            if (!res.Success)
            {
                return Json(res.Message);
            }

            if (permissionsCategory == null)
            {
                return HttpNotFound();
            }

            ViewBag.SortByName = sortByName;
            ViewBag.SortDesc = sortDesc;
            return View(permissionsCategory);
        }

        // POST: Permissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id, bool sortByName = false, bool sortDesc = false)
        {
            var getRes = service.GetByID(id);
            PermissionsCategory permissionsCategory = getRes.Entity;

            if (!getRes.Success)
            {
                return Json(getRes.Message,JsonRequestBehavior.AllowGet);
            }
            var delRes = service.Delete(permissionsCategory);
            if (!delRes.Success)
            {
                return Json(delRes.Message, JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public ActionResult PermissionCost(Guid? id)
        {
            PermissionCostViewModel viewModel = new PermissionCostViewModel();

            var resCategories = service.GetAll();

            if (!resCategories.Success)
            {
                return Json(resCategories.Message, JsonRequestBehavior.AllowGet);
            }
            if (!resCategories.Entities.Any())
            {
                return RedirectToAction("Index");
            }
            viewModel.AllCategories = resCategories.Entities.ToList();

            if (!id.HasValue)
            {
                id = resCategories.Entities.First().ID;
            }
            var resCategory = service.GetWithPermissionList(id.Value);

            if (!resCategory.Success)
            {
                return Json(resCategory.Message, JsonRequestBehavior.AllowGet);
            }

            if (resCategory.Entity == null)
            {
                return HttpNotFound();
            }
            viewModel.CurrCategory = resCategory.Entity;
            viewModel.CurrCategory.PermissionProperties =
                viewModel.CurrCategory.PermissionProperties.OrderBy(x => x.ViewOrder).ToList();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PermissionCost(PermissionCostViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var res = service.UpdatePermissions(viewModel.CurrCategory);
                viewModel.Success = res.Success;
                if (!res.Success)
                {
                    ModelState.AddModelError("", res.Message);
                }
            }
            return View(viewModel);
        }

        public ActionResult PermissionList(Guid id)
        {
            var resCategory = service.GetWithPermissionList(id);

            if (!resCategory.Success)
            {
                return Json(resCategory.Message,JsonRequestBehavior.AllowGet);
            }

            if (resCategory.Entity == null)
            {
                return HttpNotFound();
            }

            // The db object contain cuircle pointers
            resCategory.Entity.Permissions= resCategory.Entity.Permissions.Select(perm =>
            {
                var newPerm = new Permission(perm.PermissionCategoryID, perm.PermissionCost)
                {
                    ID = perm.ID,
                    PropertyValues = perm.PropertyValues.Select(val=>
                    {
                        return new PermissionPropertyValue(val);
                    }).ToList(),
                };
                return newPerm;
            }).ToList();
            return Json(resCategory.Entity.Permissions, JsonRequestBehavior.AllowGet);
        }
    }
}