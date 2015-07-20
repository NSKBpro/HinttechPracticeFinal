using HinttechPractice.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HinttechPractice.Service;
using HinttechPractice.Data;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using PagedList;
using PagedList.Mvc;

namespace HinttechPractice.Controllers
{
    ///<summary>
    ///Controller for admin actions.
    ///</summary>
    [MyAuthorizeAtribute(Roles = "Admin")]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        ///<summary>
        ///Show all registered users.
        ///</summary>
        public ActionResult ShowAllUsers(int? page)
        {
            UsersService users = new UsersService();
            List<User> model = users.FindAll().ToList();
            List<User> withoutAdmin = new List<User>();

            foreach (User user in model)
            {
                if (!user.IsUserAdmin)
                {
                    withoutAdmin.Add(user);
                }
            }
            int? currentPage = ReturnPaginationPage(withoutAdmin, page);
            return View("ShowAllUsers", withoutAdmin.ToList().ToPagedList(currentPage ?? 1, 6));
        }

        /// <summary>
        /// Return the current page for pagination.
        /// </summary>
        private int? ReturnPaginationPage(List<User> withoutAdmin, int? page)
        {
            double tempPaginationValue = withoutAdmin.Count() / 6;

            if (withoutAdmin.Count() % 6 != 0)
            {
                tempPaginationValue++;
            }

            if (tempPaginationValue < page)
            {
                page = 1;
            }

            if (page != 1 && page != tempPaginationValue && 6 * tempPaginationValue == withoutAdmin.Count())
            {
                page--;
            }

            return page;
        }

        ///<summary>
        ///Show only approved registered users.
        ///</summary>
        public ActionResult showRegOnly()
        {
            UsersService users = new UsersService();
            List<User> model = users.FindAll().ToList();
            List<User> withoutAdmin = new List<User>();

            foreach (User user in model)
            {
                if (!user.IsUserAdmin && user.IsUserRegistered)
                {
                    withoutAdmin.Add(user);
                }
            }

            return View(withoutAdmin);
        }

        ///<summary>
        ///For Admin, to confirm user registration. Also, sending mail to user.
        ///</summary>
        public ActionResult ConfirmRegistration(String id, int? page)
        {
            UsersService users = new UsersService();
            User user = (User)users.FindUserByUsername(id);

            if (user != null)
            {
                if (user.IsUserRegistered)
                {
                    user.IsUserRegistered = false;
                    Contact(user, false);
                }
                else
                {
                    user.IsUserRegistered = true;
                    Contact(user, true);
                }
                users.Edit(user);
            }

            return ShowAllUsers(page);
        }

        /// <summary>
        /// Reset all vacation days for registered users to 20.
        /// </summary>
        public ActionResult ResetVacationDays()
        {
            UsersService userService = new UsersService();
            List<User> users = userService.FindAll();

            foreach (User user in users)
            {
                user.VacationDays = 20;
                userService.Edit(user);
            }

            return RedirectToAction("ShowAllUsers");
        }

        //async Task<ActinResult>
        ///<summary>
        ///Method for sending email to user, with param 'registered'.
        ///</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(User user, Boolean registered)
        {
            if (ModelState.IsValid)
            {
                var body = "";
                var message = new MailMessage();
                message.To.Add(new MailAddress(user.Email));  // replace with valid value 
                message.From = new MailAddress("eponudaisa@gmail.com");  // replace with valid value

                if (registered)
                {
                    message.Subject = "Registration to Hinttech";
                    body = "<p>Your registration is <b>APPROVED</b>.</p>";
                }
                else
                {
                    message.Subject = "Unregistered from Hinttech";
                    body = "<p>Your registration isn't <b>APPROVED</b> anymore.</p>";
                }

                message.Body = string.Format(body);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "eponudaisa@gmail.com",  // replace with valid value
                        Password = "ra442011"  // replace with valid value
                    };

                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    //await smtp.mailSendAsync(msg);
                    smtp.Send(message);
                    return RedirectToAction("ShowAllUsers");
                }
            }
            return RedirectToAction("ShowAllUsers");
        }
    }
}