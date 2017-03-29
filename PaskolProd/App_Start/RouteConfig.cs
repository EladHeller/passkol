using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PaskolProd
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            // BotDetect requests must not be routed
            routes.IgnoreRoute("{*botdetect}", new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });

            routes.MapRoute(
                name: "Artists/",
                url: "Artists/{urlPage}",
                defaults: new { controller = "Artists", action = "UrlPage", urlPage = UrlParameter.Optional },
                 namespaces: new[] { "PaskolProd.Paskol.Controllers" }
            );

            routes.MapRoute(
                name: "Paskol",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Search", action = "Index", id = UrlParameter.Optional },
                 namespaces: new[] { "PaskolProd.Paskol.Controllers" }
            );
        }
    }
}
