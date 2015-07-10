using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HinttechPractice.Controllers
{
    ///<summary>
    ///Init controller.
    ///</summary>
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        ///<summary>
        ///Redirect to 'About' view;
        ///</summary>
        public ActionResult About()
        {
            return View();
        }

        ///<summary>
        ///Redirect to 'Contact' view.
        ///</summary>
        public ActionResult Contact()
        {
            return View();
        }

        ///<summary>
        ///Redirect to 'Login' view;
        ///</summary>
        public ActionResult LoginPage()
        {
            return View("LoginPage");
        }

        ///<summary>
        ///Redirect to 'RegisterOK' view. Like feedback.
        ///</summary>
        public ActionResult RegisterOK()
        {
            return View("RegisterOK");
        }

        ///<summary>
        ///Redirect to 'RegisterPage' view. For new users.
        ///</summary>
        public ActionResult RegisterPage()
        {
            return View("RegisterPage");
        }

        ///<summary>
        ///Error page, for unauthorized accsess.
        ///</summary>
        public ActionResult AccsessDenied()
        {
            return View("Accsess_Denied");
        }
        
	}
}