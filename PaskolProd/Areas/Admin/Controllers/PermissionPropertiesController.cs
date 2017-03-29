using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PaskolProd;
using Services;
using Model;
using Model.PriceModel;
using PaskolProd.Models;
using Model.Filters;

namespace PaskolProd.Areas.Admin.Controllers
{
    [Authorize(Roles = "SystemAdmin")]
    public class PermissionPropertiesController : Controller
    {
        private PermissionPropertyService service;
        private PermissionCategoryService CategoryService;

        public PermissionPropertiesController()
        {
            CategoryService = ServiceLocator.GetService<PermissionCategoryService>();
            service = ServiceLocator.GetService<PermissionPropertyService>();
        }
        // GET: PermissionProperties
        public ActionResult Index(Guid id, bool sortByName =false, bool sortDesc = false)
        {
            var res = CategoryService.GetByID(id, PermissionNavigationOptions.IncludeProperties);
            if (!res.Success)
            {
                return Json(res.Message, JsonRequestBehavior.AllowGet);
            }
            else
            {
                IEnumerable<PermissionProperty> props = res.Entity
                    ?.PermissionProperties
                    ?.Where(prop=>!prop.IsCostLevel);
                if (props != null && props.Any())
                {
                    
                    if (sortDesc)
                    {
                        if (sortByName)
                        {
                            props = props.OrderByDescending(x => x.Name);
                        }
                        else
                        {
                            props = props.OrderByDescending(x => x.ViewOrder);
                        }
                    }
                    else
                    {
                        if (sortByName)
                        {
                            props = props.OrderBy(x => x.Name);
                        }
                        else
                        {
                            props = props.OrderBy(x => x.ViewOrder);
                        }
                    }
                }

                ViewBag.SortByName = sortByName;
                ViewBag.SortDesc = sortDesc;
                ViewBag.CategoryID = res.Entity.ID;
                ViewBag.CategoryName = res.Entity.Name;
                return View(props);
            }
        }


        // GET: PermissionProperties/Create
        public ActionResult Create(Guid id)
        {
            PermissionProperty perProp = new PermissionProperty();
            perProp.ViewOrder = 10;
            perProp.IsActive = true;
            perProp.PermissionsCategoryID = id;
            return View(perProp);
        }

        // POST: PermissionProperties/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PermissionsCategoryID,Name,IsActive,ViewOrder")] PermissionProperty permissionProperty)
        {
            if (ModelState.IsValid)
            {
                permissionProperty.ID = Guid.NewGuid();
                var res = service.Add(permissionProperty);
                if (!res.Success)
                {
                    ModelState.AddModelError("", res.Message);
                }

                return RedirectToAction("Index", new { id = permissionProperty.PermissionsCategoryID});
            }

            return View(permissionProperty);
        }

        // GET: PermissionProperties/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var res = service.GetByID(id.Value, true);
            if (!res.Success)
            {
                return Json(res.Message, JsonRequestBehavior.AllowGet);
            }

            PermissionProperty permissionProperty = res.Entity;
            if (permissionProperty == null)
            {
                return HttpNotFound();
            }

            permissionProperty.PropertyValues = permissionProperty.PropertyValues ?? 
                new List<PermissionPropertyValue>();

            return View(new PermissionPropertyViewModel(permissionProperty));
        }

        // POST: PermissionProperties/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PermissionPropertyViewModel permissionProperty)
        {
            if (ModelState.IsValid)
            {
                PermissionProperty updateItem = permissionProperty as PermissionProperty;

                updateItem.PropertyValues = permissionProperty.PropertyValueViews
                                              ?.Where(val => !val.IsDeleted)
                                             .Select(val => new PermissionPropertyValue(val))
                                             .ToList();
                
                var res = service.Update(updateItem);
                if (res.Success)
                {
                   return RedirectToAction("Index", new { id = permissionProperty.PermissionsCategoryID});
                }
                else
                {
                    ModelState.AddModelError("", res.Message);
                }
                
            }
            return View(permissionProperty);
        }

        // GET: PermissionProperties/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var res = service.GetByID(id.Value, false);
            if (!res.Success)
            {
                return Json(res.Message, JsonRequestBehavior.AllowGet);
            }

            PermissionProperty permissionProperty = res.Entity;
            if (permissionProperty == null)
            {
                return HttpNotFound();
            }

            return View(permissionProperty);
        }

        // POST: PermissionProperties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {

            var res = service.GetByID(id, false);
            if (!res.Success)
            {
                return Json(res.Message, JsonRequestBehavior.AllowGet);
            }

            PermissionProperty permissionProperty = res.Entity;
            if (permissionProperty == null)
            {
                return HttpNotFound();
            }
            Guid categoryId = permissionProperty.PermissionsCategoryID;
            service.Delete(permissionProperty);
            
            return RedirectToAction("Index", new { id = categoryId });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
