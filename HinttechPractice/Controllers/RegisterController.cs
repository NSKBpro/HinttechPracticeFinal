using HinttechPractice.Models;
using HinttechPractice.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HinttechPractice.Data;
using System.Web.Security;
using Newtonsoft.Json;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HinttechPractice.Controllers
{
    ///<summary>
    ///Controller for register new users, edit users, change password...
    ///</summary>
    public class RegisterController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult RegisterPage()
        {
            return View();
        }

        ///<summary>
        ///Register new users, and upload picture(if exists).
        ///</summary>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult RegisterPage(RegisterViewModel model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                UsersService userService = new UsersService();
                User currentUser = userService.FindUserByUsername(model.Username);

                if (currentUser != null)
                {
                    ViewBag.Error = "Username already exist!";
                    return View();
                }
                if (!model.Password.Equals(model.repeatPassword))
                {
                    ViewBag.Error = "Password doesn't match!";
                    return View();
                }
                currentUser = userService.FindUserByEmail(model.Email);
                if (currentUser != null)
                {
                    ViewBag.Error = "Email address already exist!!";
                    return View();
                }

                User user = new User();
                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Password = model.Password;
                user.Username = model.Username;
                user.DateCreated = DateTime.Now;
                user.IsUserAdmin = false;
                user.IsUserRegistered = false;
                user.LastLoginDate = DateTime.Now;
                user.VacationDays = 20;

                if (file != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                        if (array.Length > 55000)
                        {
                            ViewBag.Error = "Image size can't be more than ~50Kb!!";
                            return View();
                        }
                        user.ProfilePicture = array;
                    }

                }
                userService.Create(user);
                return RedirectToAction("RegisterOK", "Home");
            }
            return View();
        }

        [Authorize]
        public ActionResult EditPage(String id)
        {
            UsersService users = new UsersService();
            User user = (User)users.FindUserByUsername(id);
            String currentUsername = User.Identity.Name;
            if (user != null)
            {
                if (!currentUsername.Equals(user.Username))
                {
                    return RedirectToAction("Index", "Home");
                }

                EditAccountViewModel model = new EditAccountViewModel();
                model.Email = user.Email;
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.removeImage = false;
                return View(model);
            }
            else
            {
                return View();
            }
        }

        ///<summary>
        ///Edit users.
        ///</summary>
        [HttpPost]
        [Authorize]
        public ActionResult EditPage(EditAccountViewModel model, HttpPostedFileBase file)
        {
            UsersService users = new UsersService();
            String currentUsername = User.Identity.Name;
            User user = (User)users.FindUserByUsername(currentUsername);

            if (user != null)
            {
                if (!currentUsername.Equals(user.Username))
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            User checkForMail = (User)users.FindUserByEmail(model.Email);
            if (checkForMail == null || checkForMail.Email.Equals(user.Email))
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                if (!model.removeImage)
                {
                    if (file != null)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            file.InputStream.CopyTo(ms);
                            byte[] array = ms.GetBuffer();
                            if (array.Length > 55000)
                            {
                                ViewBag.Error = "Image size can't be more than ~50Kb!!";
                                return View(model);
                            }
                            user.ProfilePicture = array;
                        }
                    }
                }
                else
                {
                    user.ProfilePicture = null;
                }

                users.Edit(user);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Email address already exist!!";
                return View(model);
            }
        }

        [Authorize]
        public ActionResult ChangePassPage(String id)
        {
            UsersService users = new UsersService();
            User user = (User)users.FindUserByUsername(id);
            String currentUsername = User.Identity.Name;
            if (!currentUsername.Equals(user.Username))
            {
                return RedirectToAction("Index", "Home");
            }
            ChangePasswordViewModel model = new ChangePasswordViewModel();
            return View(model);
        }

        ///<summary>
        ///Change user password.
        ///</summary>
        [HttpPost]
        [Authorize]
        public ActionResult ChangePassPage(ChangePasswordViewModel model)
        {
            UsersService users = new UsersService();
            String currentUsername = User.Identity.Name;
            User user = (User)users.FindUserByUsername(currentUsername);

            if (user.Password.Equals(model.currentPassword))
            {
                if (model.newPassword.Equals(model.confirmNewPass))
                {
                    user.Password = model.confirmNewPass;
                    users.Edit(user);
                }
                else
                {
                    ViewBag.Error = "Password didn't match!!!";
                    return View(model);
                }
            }
            else
            {
                ViewBag.Error = "Password incorect!!";
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        ///<summary>
        ///Show account details.
        ///</summary>
        [Authorize]
        public ActionResult AccountDetails(String id)
        {
            UsersService users = new UsersService();
            User user = (User)users.FindUserByUsername(id);
            String currentUsername = User.Identity.Name;

            if (user != null)
            {
                if (!currentUsername.Equals(user.Username))
                {
                    return RedirectToAction("Index", "Home");
                }
                UserViewModel model = new UserViewModel();
                model.Email = user.Email;
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.DateCreated = (DateTime)user.DateCreated;
                model.IsUserRegistered = user.IsUserRegistered;
                model.ProfilePicture = user.ProfilePicture;
                model.Username = user.Username;
                model.VacationDays = user.VacationDays;
                return View(model);
            }
            else
            {
                return View();
            }
        }
    }
}