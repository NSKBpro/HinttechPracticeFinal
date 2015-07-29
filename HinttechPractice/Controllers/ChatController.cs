using HinttechPractice.Models;
using HinttechPractice.Data;
using HinttechPractice.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HinttechPractice.Controllers
{
    public class ChatController : Controller
    {
        private UsersService usr = new UsersService();
        public static List<UsersLite> usersOnline;
        public ActionResult Chat()
        {
            string usrnmOfUserLoggedIn = HttpContext.User.Identity.Name;
            int userId = usr.FindUserByUsername(usrnmOfUserLoggedIn).UserId;
            ViewBag.idOfUser = userId;
            List<UsersLite> users = new List<UsersLite>();
            
            if (usersOnline!=null)
            {

                foreach (UsersLite us in usersOnline)
                {
                    if (us.username == usrnmOfUserLoggedIn)
                    {
                        us.activity = true;
                        usersOnline.First(d => d.username == usrnmOfUserLoggedIn).activity = true;
                       

                    }
                    if (us.lastSeenOn == DateTime.Now.ToString("yyyy-MM-dd"))
                    {
                        us.lastSeenOn = "Today";
                    }
                
                    ViewBag.users = usersOnline;
                }

            }
            else
            {
                foreach (User us in usr.FindAll())
                {
                    UsersLite usl = new UsersLite();
                    usl.usrId = us.UserId;
                    usl.firstName = us.FirstName;
                    usl.lastName = us.LastName;
                    usl.username = us.Username;
                    usl.profilePicture = us.ProfilePicture;
                    if (us.Username == usrnmOfUserLoggedIn)
                    {
                        usl.activity = true;
                    }
                    if (((DateTime)us.LastLoginDate).ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
                    {
                        usl.lastSeenOn = "Today";
                    }
                    else if (((DateTime)us.LastLoginDate).ToString("yyyy-MM-dd") == (DateTime.Now.AddDays(-1)).ToString("yyyy-MM-dd"))
                    {
                        usl.lastSeenOn = "Yesterday";

                    }else
                    {
                        usl.lastSeenOn = ((DateTime)us.LastLoginDate).ToString("yyyy-MM-dd");
                    }
                    users.Add(usl);
                }
                usersOnline = new List<UsersLite>();
                usersOnline = users;
                ViewBag.users = users;
            }
     
           
            return View();
        }
	}
}