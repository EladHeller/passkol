using Model.Filters;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaskolProd.Areas.Admin.Controllers
{
    [Authorize(Roles = "SystemAdmin")]
    public class DashboardController : Controller
    {
        DashboardService service = ServiceLocator.GetService<DashboardService>();

        // GET: AdminDashboard
        public ActionResult Index()
        {
            var res = service.GetDashboardData();
            if (!res.Success)
            {
                ViewBag.Message = res.Message;
            }

            return View(res.Entity);
        }
    }
}