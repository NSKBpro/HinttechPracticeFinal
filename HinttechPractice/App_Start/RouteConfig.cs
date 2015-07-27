using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HinttechPractice
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "loginPage",
                url: "home/login",
                defaults: new { controller = "Login", action = "LoginPage", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "editPage",
                url: "home/register",
                defaults: new { controller = "Register", action = "RegisterPage", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "registerPage",
                url: "home/edit",
                defaults: new { controller = "Register", action = "EditPage", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "changePass",
               url: "home/changePassword",
               defaults: new { controller = "Register", action = "ChangePassPage", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "home",
                url: "Home/home",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                 name: "error401",
                 url: "home/error",
                 defaults: new { controller = "Home", action = "AccsessDenied", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                 name: "error404",
                 url: "home/error404",
                 defaults: new { controller = "Home", action = "Error404", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
