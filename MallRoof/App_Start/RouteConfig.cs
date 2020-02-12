﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MallRoof
{
    public class RouteConfig
    {
        public static void 
            RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("Manage/");
            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    //defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //    defaults: new { controller = "Premises", action = "Index", id = UrlParameter.Optional }

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                //defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                new { controller = "Premises", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute("MyRoute", "{controller}/{action}", defaults: new { action = "Index" });
        }


    }
}
