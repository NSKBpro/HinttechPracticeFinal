using HinttechPractice.Data;
using HinttechPractice.Data.DataContext;
using HinttechPractice.Security;
using HinttechPractice.Service;
using HotelAdvisor.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HinttechPractice.Controllers
{
    ///<summary>
    ///All login actions.
    ///</summary>
    [AllowAnonymous]
    public class LoginController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoginPage()
        {
            return View();
        }


        ///<summary>
        ///Post method for login user(on context).
        ///</summary>
        [HttpPost]
        public ActionResult LoginPage(LoginViewModel user)
        {
            if (user.Password == null || user.UserName == null) return View();
            UsersService userService = new UsersService();
            User currentUser = userService.FindUserByUsername(user.UserName);

            if (currentUser == null)
            {
                ViewBag.Error = "Wrong username..!";
                return View();
            }
            else
            {
                if (!currentUser.Password.Equals(user.Password))
                {
                    ViewBag.Error = "Wrong password combination!";
                    return View();
                }
                if (!currentUser.IsUserRegistered)
                {
                    if (!currentUser.IsUserAdmin) { 
                    ViewBag.Error = "Your account isn't approved yet.Check your email.";
                    return View();
                    }
                }
                byte[] tempPicture = currentUser.ProfilePicture; 
                currentUser.ProfilePicture = null; // set picture to null, for JsonConverter.
                ICollection<Vacation> tempVacation =  currentUser.Vacations;
                ICollection<Holiday> tempHolidays = currentUser.Holidays;
                currentUser.Vacations = null;
                currentUser.Holidays = null;
                FormsAuthenticationTicket fat = new FormsAuthenticationTicket(1, user.UserName, DateTime.Now, DateTime.Now.AddMinutes(15), false,
                    JsonConvert.SerializeObject(currentUser, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        })
                    );
                HttpCookie ck = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(fat));
                ck.Expires = DateTime.Now.AddMinutes(15);

                Response.Cookies.Add(ck);
                currentUser.LastLoginDate = DateTime.Now;
                currentUser.ProfilePicture = tempPicture;
                currentUser.Holidays = tempHolidays;
                currentUser.Vacations = tempVacation;
                userService.Edit(currentUser);
                ViewBag.Error = "";
                return RedirectToRoute("home");
            }
        }

        ///<summary>
        ///Logging out user from context(clear cookie).
        ///</summary>
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            HttpCookie ck = Request.Cookies[FormsAuthentication.FormsCookieName];
            ck.Expires = DateTime.Now;
            return RedirectToRoute("home");
        }
    }
}