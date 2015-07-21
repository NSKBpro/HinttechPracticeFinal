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
using Microsoft.AspNet.SignalR;

namespace HinttechPractice
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalFilters.Filters.Add(new System.Web.Mvc.AuthorizeAttribute());
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

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
    }
}
