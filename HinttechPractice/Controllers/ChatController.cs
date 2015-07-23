using HinttechPractice.Models;
using HinttechPractice.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HinttechPractice.Data;

namespace HinttechPractice.Controllers
{
    public class ChatController : Controller
    {
        private UsersService usr = new UsersService();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Chat()
        {
            string usrnmOfUserLoggedIn = HttpContext.User.Identity.Name;
            int userId = usr.FindUserByUsername(usrnmOfUserLoggedIn).UserId;
            ViewBag.idOfUser = userId;
            List<UsersLite> users = new List<UsersLite>();

            foreach (User us in usr.FindAll())
            {
                UsersLite usl = new UsersLite();
                usl.usrId = us.UserId;
                usl.firstName = us.FirstName;
                usl.lastName = us.LastName;
                usl.username = us.Username;
                users.Add(usl);
            }

            ViewBag.users = users;
            return View();
        }

	}
}