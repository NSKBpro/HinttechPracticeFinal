using HinttechPractice.Data;
using HinttechPractice.Security;
using HinttechPractice.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HinttechPractice.Controllers
{
    
    public class LoadHolidaysController : Controller
    {
        private HolidayService db = new HolidayService();
        TestService db2 = new TestService();
        private UsersService usr = new UsersService();
        //
        // GET: /LoadHolidays/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult initHolidays()
        {
            ViewBag.initHolidays = db.GetHolidays();
            ViewBag.initVacations = db2.GetVacations();
            string usrnmOfUserLoggedIn = HttpContext.User.Identity.Name;
            ViewBag.idOfUser = usr.FindUserByUsername(usrnmOfUserLoggedIn).UserId;
            return View("InitCalendar");
        }

        private DateTime dt;

        [HttpGet]
        [MyAuthorizeAtribute(Roles = "Admin")]
        public ActionResult ViewForDate()
        {
            dt = Convert.ToDateTime(Request.QueryString["dateFrom"]);
            ViewBag.dan = Request.QueryString["dateFrom"];
            string usrnmOfUserLoggedIn = HttpContext.User.Identity.Name;
            ViewBag.idOfUser = usr.FindUserByUsername(usrnmOfUserLoggedIn).UserId;
            bool isEmpty = !db.GetHolidaysForDate(dt).Any();
            if (isEmpty)
            {
                ViewBag.prazanDan = "da";
            } else {
                ViewBag.prazanDan = null;
            }
            return View("WindowForDay", db.GetHolidaysForDate(dt));
        }

        [HttpPost]
        [MyAuthorizeAtribute(Roles = "Admin")]
        public ActionResult AddHoliday(Holiday h)
        {
            db.AddHoliday(h);
            ViewBag.initHolidays = db.GetHolidays();
            ViewBag.initVacations = db2.GetVacations();
            return View("InitCalendar");
        }

        [HttpPost]
        [MyAuthorizeAtribute(Roles = "Admin")]
        public ActionResult RemoveHoliday(int HolidayId)
        {
            db.RemoveHoliday(HolidayId);
            ViewBag.initHolidays = db.GetHolidays();
            ViewBag.initVacations = db2.GetVacations();
            return View("InitCalendar");
        }

        [HttpPost]
        [MyAuthorizeAtribute(Roles = "Admin")]
        public ActionResult EditHoliday(Holiday h)
        {
            db.EditHoliday(h);
            ViewBag.initHolidays = db.GetHolidays();
            ViewBag.initVacations = db2.GetVacations();
            return View("InitCalendar");
        }

        [HttpPost]
        [MyAuthorizeAtribute(Roles = "Admin")]
        public ActionResult EditHolidayRedirect(int HolidayId)
        {
            ViewBag.holidayForEdit = db.GetHolidays().Find(HolidayId);
            return View("EditHoliday");
        }
	}
}