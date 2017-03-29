using System.Net;
using System.Web.Mvc;
using Services;
using PaskolProd;
using Services.Messaging.Responses;
using System.Collections.Generic;
using PaskolProd.Models;
using System.Linq;
using Antlr.Runtime.Misc;
using System;
using Model;
using Model.Filters;

namespace PaskolProd.Areas.Admin.Controllers
{
    [Authorize(Roles = "SystemAdmin,AdminLabel")]
    public class TagsController : Controller
    {
        private TagService service = ServiceLocator.GetService<TagService>(); 
        // GET: Tags
        public ActionResult Index(Guid? id, bool? success, bool sortByName=false, bool sortDesc = false)
        {
            ResponseBase res = null;
            TagViewModel viewModel = null;
            if (id.HasValue)
            {
                res = service.GetByID(id.Value, 
                    ModelGetNavigtorOptions.GetNavigatorRecursive, 
                    ModelGetNavigtorOptions.GetNavigator);
            }
            else
            {
                res = service.GetAllByParentId(id);
            }
            if (!res.Success)
            {
                ViewBag.Success = false;
                ViewBag.Message = "אירעה שגיאה בעת הבאת התגיות.";
                viewModel = new TagViewModel();
                viewModel.TagList = new List<Tag>();
            }
            else
            {
                Tag tag = null;

                if (id.HasValue)
                {
                    tag = (res as EntityResponse<Tag>).Entity;
                }
                else
                {
                    tag = new Tag();
                    tag.TagList = (res as EntityAllResponse<Tag>).Entities as ICollection<Tag>;
                }

                if (tag.TagList != null && tag.TagList.Count > 0)
                {
                    if (sortByName)
                    {
                        if (sortDesc)
                        {
                            tag.TagList = tag.TagList.OrderByDescending(tg => tg.Name).ToList();
                        }
                        else
                        {
                            tag.TagList = tag.TagList.OrderBy(tg => tg.Name).ToList();
                        }
                    }
                    else
                    {
                        if (sortDesc)
                        {
                            tag.TagList = tag.TagList.OrderByDescending(tg => tg.ViewOrder).ToList();
                        }
                        else
                        {
                            tag.TagList= tag.TagList.OrderBy(tg => tg.ViewOrder).ToList();
                        }
                    }
                }

                ViewBag.Success = success;

                viewModel = 
                    new TagViewModel(tag,
                        id, 
                        true, 
                        true, 
                        sortByName,
                        sortDesc);
            }

            return View(viewModel);
        }

        // GET: Tags/Create
        public ActionResult Create(Guid? parentId, TagLevel parentLevel, bool sortByName = false, bool sortDesc = false)
        {
            var tag = new TagViewModel()
            {
                ParentTagID = parentId,
                Level = parentLevel + 1,
                IsCanDelete = true,
                IsDynamic = false,
                IsPublicTag = true,
                ViewOrder = 30,
                SortByName =sortByName,
                SortDesc = sortDesc
            };
            return View(tag);
        }

        // POST: Tags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,IsCanDelete,IsPublicTag,IsDynamic,Level,ViewOrder,ParentTagID")] Tag tag, bool sortByName = false, bool sortDesc = false)
        {
            if (ModelState.IsValid)
            {
                var res = service.Add(tag);
                if (!res.Success)
                {
                    ModelState.AddModelError("", res.Message);
                }
                else
                {
                    return RedirectToAction("Index", "tags", new { id = tag.ParentTagID ,success=true, sortByName = sortByName , sortDesc = sortDesc});
                }
            }

            return View(tag);
        }

        // GET: Tags/Edit/5
        public ActionResult Edit(Guid? id, bool sortByName = false, bool sortDesc = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var res = service.GetByID(id.Value);

            if (!res.Success)
            {
                return this.Json(res.Message,JsonRequestBehavior.AllowGet);
            }

            TagViewModel tag = new TagViewModel(res.Entity,id,false,false,sortByName,sortDesc);
            return View(tag);
        }

        // POST: Tags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,IsCanDelete,ParentTagID,IsPublicTag,IsDynamic,Level,ViewOrder,CreateDate")] TagViewModel tag, bool sortByName = false, bool sortDesc = false)
        {
            if (ModelState.IsValid)
            {
                var res = service.Update(tag);
                if (!res.Success)
                {
                    ModelState.AddModelError("", res.Message);
                }
                else
                {
                    return RedirectToAction("Index", "tags", new { id = tag.ParentTagID, success = true, sortByName = sortByName, sortDesc = sortDesc });
                }
            }

            return View(tag);
        }

        // GET: Tags/Delete/5
        public ActionResult Delete(Guid? id, bool sortByName = false, bool sortDesc = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var res = service.GetByID(id.Value);

            if (!res.Success)
            {
                return this.Json(res.Message,JsonRequestBehavior.AllowGet);
            }

            TagViewModel tag = new TagViewModel(res.Entity, id, false, false, sortByName, sortDesc);
            return View(tag);
        }

        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(TagViewModel tg, bool sortByName = false, bool sortDesc = false)
        {
            var getRes = service.GetByID(tg.ID,
                ModelGetNavigtorOptions.None,
                ModelGetNavigtorOptions.GetNavigatorRecursive);

            if (!getRes.Success)
            {
                return this.Json(getRes.Message, JsonRequestBehavior.AllowGet);
            }

            Tag tag = getRes.Entity;
            var parentTagId = tag.ParentTagID;
            var res = service.Delete(tag);
            if (!res.Success)
            {
                ModelState.AddModelError("", res.Message);
                return View(tg);
            }

            return RedirectToAction("Index","tags", new { id = parentTagId, success = true, sortByName = sortByName, sortDesc = sortDesc });
        }

        public ActionResult GetBySearch(string tagName)
        {
            var getRes = service.GetBySearch(tagName);

            if (!getRes.Success)
            {
                return new HttpStatusCodeResult(500, getRes.Message);
            }
            IEnumerable<TagViewModel> tagsViewModel = null;
            try
            {
                tagsViewModel = getRes.Entities?.Select(tg => new TagViewModel(tg, tg.ID, true, false, false, false));
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(500, e.ToString());
            }
            return Json(tagsViewModel, JsonRequestBehavior.AllowGet);
        }
    }
}
