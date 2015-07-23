using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Security;
using System.Web.SessionState;
using HinttechPractice.Models;
using HinttechPractice.Security;
using System.Web.Security;
using Newtonsoft.Json;
using HinttechPractice.Data;
using System.Web.Optimization;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.AspNet.SignalR;
using log4net;
using HinttechPractice.Core;
using HinttechPractice.Core.Exceptions;

namespace HinttechPractice
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //private logger class for MyLogger in this class.
        private ILog log;

        protected void Application_Start()
        {

            //Always call this when need to use logger.
            log4net.GlobalContext.Properties["LogName"] = DateTime.Now.ToShortDateString().ToString();
            log = MyLogger.GetLogger(typeof(MvcApplication));
            log.Info("Aplication started");


            GlobalFilters.Filters.Add(new System.Web.Mvc.AuthorizeAttribute());
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var idProvider = new PrincipalUserIdProvider();
            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => idProvider);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie ck = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (ck != null)
            {
                FormsAuthenticationTicket fat = FormsAuthentication.Decrypt(ck.Value);
                User user = JsonConvert.DeserializeObject<User>(fat.UserData);
                MyPrincipal mp = new MyPrincipal(user.Username);
                mp.Account = user;
                HttpContext.Current.User = mp;
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            log4net.GlobalContext.Properties["LogName"] = DateTime.Now.ToShortDateString().ToString();
            log = MyLogger.GetLogger(typeof(MvcApplication));

            Exception ex = Server.GetLastError();

            //Checking if exception was 404
            if (ex.GetType() == typeof(HttpException))
            {
                HttpContext.Current.Server.ClearError();
                HttpContext.Current.Response.RedirectToRoute("error404");
                throw new Exception404PageNotFound("Error 404, Page not found!", ex);
            }
            else
            {
                log.Error("Application error caught", ex);
            }
        }

    }
}
