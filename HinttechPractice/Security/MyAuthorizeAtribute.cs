using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security;
using System.Web.Mvc;
using HinttechPractice.Data;

namespace HinttechPractice.Security
{
    public class MyAuthorizeAtribute:AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                MyPrincipal mp = HttpContext.Current.User as MyPrincipal;
                if (!mp.IsInRole(Roles))
                {
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new{
                        controller = "Login", action="LoginPage"}));
                }
            }
        }

    }
}