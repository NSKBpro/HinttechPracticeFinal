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
    [MyAuthorizeAtribute(Roles = "User")]
    public class LoadVacationsController : Controller
    {
        private static string s1;
        private static string s2;
        private HolidayService db2 = new HolidayService();
        TestService db = new TestService();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetVacations()
        {

            ViewBag.Title = "CalendarView";
            ViewBag.initHolidays = db2.GetHolidays();
            ViewBag.initVacations = db.GetVacations();
            return View("InitCalendar");
        }

        public ActionResult RegistracijaOdmora(String parameterdatum1, String parameterdatum2)
        {
            UsersService users = new UsersService();
            User u = users.FindUserByUsername(HttpContext.User.Identity.Name);

            s1 = parameterdatum1;
            s2 = parameterdatum2;
            ViewBag.Parameterdatum1 = parameterdatum1;
            ViewBag.Parameterdatum2 = parameterdatum2;
            ViewBag.UserId = u.UserId;
            var vac = new Vacation();
            vac.UserId = Int32.Parse(u.UserId.ToString());
            vac.DateFrom = Convert.ToDateTime(parameterdatum1);
            vac.DateTo = Convert.ToDateTime(parameterdatum2);


            return View(vac);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistracijaOdmora(Vacation vacation)
        {
            UsersService users = new UsersService();
            User u = (User)users.FindById(vacation.UserId);
            Double numDays = (vacation.DateTo - vacation.DateFrom).TotalDays;
            if (Convert.ToInt32(numDays) < u.VacationDays)
            {

                if (ModelState.IsValid)
                {
                    vacation.DateFrom = vacation.DateFrom;
                    vacation.DateTo = vacation.DateTo;
                    db.AddVacation(vacation);  
                    int days = u.VacationDays - Convert.ToInt32(numDays);
                    u.VacationDays = days;
                    users.Edit(u);
                    return RedirectToAction("initHolidays", "LoadHolidays");

                }
                return View();
            }
            else
            {
                return RedirectToAction("initHolidays", "LoadHolidays");
            }
        }

        public ActionResult EditVacation(int vacationId)
        {
            UsersService users = new UsersService();
            User u = users.FindUserByUsername(HttpContext.User.Identity.Name);
            TestService ser = new TestService();
            if (ser.FindVacationByUserId(u.UserId, vacationId) == true)
            {
                Vacation vac = (Vacation)db.FindById(vacationId);
                if (vac == null)
                {
                    //...
                    return RedirectToAction("initHolidays", "LoadHolidays");
                }
                String datum = DateTime.Now.ToString("yyyy-MM-dd");
                ViewBag.Datum = datum;
             
                ViewBag.editDateFrom = vac.DateFrom.ToString("yyyy-MM-dd");
                ViewBag.editDateTo = vac.DateTo.ToString("yyyy-MM-dd");
                return View(vac);
            }
            else
            {
                return RedirectToAction("initHolidays", "LoadHolidays");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditVacation(Vacation vacation)
        {
            UsersService users = new UsersService();
            User u = (User)users.FindById(vacation.UserId);
            Double numDays = (vacation.DateTo - vacation.DateFrom).TotalDays;
            if (Convert.ToInt32(numDays) < u.VacationDays)
            {
                if (ModelState.IsValid)
                {
                    db.EditVacation(vacation);
                    return RedirectToAction("initHolidays", "LoadHolidays");

                }
                return View();
            }else
            {
                return RedirectToAction("initHolidays", "LoadHolidays");
            }
        }

        public ActionResult DeleteVacation(int vacationId)
        {
            Vacation vac = (Vacation)db.FindById(vacationId);
            return View(vac);
        }

        [HttpPost]
        public ActionResult DeleteVacation(Vacation vacation)
        {
                double numDays = (vacation.DateTo - vacation.DateFrom).TotalDays;
                UsersService users = new UsersService();
                User u = (User)users.FindUserByUsername(HttpContext.User.Identity.Name);
                int days = u.VacationDays + Convert.ToInt32(numDays);
                u.VacationDays = days;
                users.Edit(u);
                db.DeleteVacation(vacation.VacationPeriodId);
                return RedirectToAction("initHolidays", "LoadHolidays");
        }
    }
}