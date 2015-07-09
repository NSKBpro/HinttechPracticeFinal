using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HinttechPractice.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult LoginPage()
        {
            return View("LoginPage");
        }
        public ActionResult RegisterOK()
        {
            return View("RegisterOK");
        }

        public ActionResult RegisterPage()
        {
            return View("RegisterPage");
        }

        public ActionResult AccsessDenied()
        {
            return View("Accsess_Denied");
        }
        
	}
}