using HinttechPractice.Data;
using HinttechPractice.Data.DataContext;
using HinttechPractice.Models;
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
        public static string currentUserOffline;
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
        [ValidateAntiForgeryToken]
        public ActionResult LoginPage(LoginViewModel user)
        {
            if (user.Password == null || user.UserName == null)
            {
                return View();
            }

            UsersService userService = new UsersService();
            User currentUser = userService.FindUserByUsername(user.UserName);
            currentUserOffline = currentUser.Username;

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
                    if (!currentUser.IsUserAdmin)
                    {
                        ViewBag.Error = "Your account isn't approved yet.Check your email.";
                        return View();
                    }
                }

                byte[] tempPicture = currentUser.ProfilePicture;
                currentUser.ProfilePicture = null; // set picture to null, for JsonConverter.
                ICollection<Vacation> tempVacation = currentUser.Vacations;
                ICollection<Holiday> tempHolidays = currentUser.Holidays;
                ICollection<ChatRoom> tempChatRoom = currentUser.ChatRooms;
                ICollection<ChatRoomMessage> tempChatRoomMessage = currentUser.ChatRoomMessages;
                ICollection<Notification> tempNotification = currentUser.Notifications;

                currentUser.Vacations = null;
                currentUser.Holidays = null;
                currentUser.ChatRooms = null;
                currentUser.ChatRoomMessages = null;
                currentUser.Notifications = null;
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
                currentUser.ChatRooms = tempChatRoom;
                currentUser.ChatRoomMessages = tempChatRoomMessage;
                currentUser.Notifications = tempNotification;
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
            if (ChatController.usersOnline != null)
            {
                foreach (UsersLite us in ChatController.usersOnline)
                {
                    if (us.username == currentUserOffline)
                    {
                        us.activity = true;
                        ChatController.usersOnline.First(d => d.username == currentUserOffline).activity = false;
                    }

                    ViewBag.users = ChatController.usersOnline;
                }
            }
            FormsAuthentication.SignOut();
            HttpCookie ck = Request.Cookies[FormsAuthentication.FormsCookieName];
            ck.Expires = DateTime.Now;
            return RedirectToRoute("home");
        }
    }
}