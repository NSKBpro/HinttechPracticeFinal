using HinttechPractice.Data;
using HinttechPractice.Models;
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

        public ActionResult initHolidays(int? page)
        {
            ViewBag.initHolidays = db.GetHolidays();
            ViewBag.initVacations = db2.GetVacations();
            List<UsersLite> users = new List<UsersLite>();

            foreach (User us in usr.FindAll())
            {
                UsersLite usl = new UsersLite();
                usl.usrId = us.UserId;
                usl.firstName = us.FirstName;
                usl.lastName = us.LastName;

                users.Add(usl);
            }

            ViewBag.users = users;
            string usrnmOfUserLoggedIn = HttpContext.User.Identity.Name;
            int userId=usr.FindUserByUsername(usrnmOfUserLoggedIn).UserId;
            ViewBag.idOfUser = userId;
            User u=(User)usr.FindById(userId);
            ViewBag.BrDana = u.VacationDays;
            String datum = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.Datum = datum;
            return View("InitCalendar");
        }

        private DateTime dt;

        [HttpGet]
        [MyAuthorizeAtribute(Roles = "Admin")]
        public ActionResult ViewForDate()
        {
            dt = Convert.ToDateTime(Request.QueryString["dateFrom"]);
            ViewBag.dan = Request.QueryString["dateFrom"];
            ViewBag.danDo = Request.QueryString["dateTo"];
            string usrnmOfUserLoggedIn = HttpContext.User.Identity.Name;
            ViewBag.idOfUser = usr.FindUserByUsername(usrnmOfUserLoggedIn).UserId;
            List<HolidaysAndUsers> haus = new List<HolidaysAndUsers>();

            
            
            foreach (Holiday h in db.GetHolidaysForDate(dt))
            {
                HolidaysAndUsers hau = new HolidaysAndUsers();
                User tempUser = (User)usr.FindById(h.UserId);
                hau.firstName = tempUser.FirstName;
                hau.lastName = tempUser.LastName;
                hau.HolidayId = h.HolidayId;
                hau.DateFrom = h.DateFrom;
                hau.DateTo = h.DateTo;
                hau.Descrition = h.Description;
                haus.Add(hau);
            }

            bool isEmpty = !db.GetHolidaysForDate(dt).Any();
            if (isEmpty)
            {
                ViewBag.prazanDan = "da";
            } else {
                ViewBag.prazanDan = null;
            }
            return View("WindowForDay", haus);
        }

        [HttpPost]
        [MyAuthorizeAtribute(Roles = "Admin")]
        public ActionResult AddHoliday(Holiday h)
        {
            if (h.DateFrom <= h.DateTo)
            {
                db.AddHoliday(h);
                ViewBag.initHolidays = db.GetHolidays();
                ViewBag.initVacations = db2.GetVacations();
                return View("InitCalendar");
            }
            else
            {
                ViewBag.errorMessage = "Date from can't be greater that date to.";
                ViewBag.initHolidays = db.GetHolidays();
                ViewBag.initVacations = db2.GetVacations();
                return View("InitCalendar");
            }

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
            ViewBag.holidayForEditTo = db.GetHolidays().Find(HolidayId).DateTo.ToString("yyyy-MM-dd");
            ViewBag.holidayForEditFrom = db.GetHolidays().Find(HolidayId).DateFrom.ToString("yyyy-MM-dd");
            return View("EditHoliday");
        }
	}
}